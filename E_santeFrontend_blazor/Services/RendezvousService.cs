using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using E_santeFrontend.Models;
using System.Text.Json;

namespace E_santeFrontend.Services
{
    public class RendezvousService
    {
        private readonly HttpClient _http;
        private readonly E_santeFrontend.Helpers.LocalStorageService _storage;

        public RendezvousService(HttpClient http, E_santeFrontend.Helpers.LocalStorageService storage)
        {
            _http = http;
            _storage = storage;
        }

        private async Task EnsureAuthHeaderAsync()
        {
            try
            {
                var token = await _storage.GetItemAsync("auth_token");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch { }
        }

        public async Task<List<RendezvousDto>> GetRendezvousAsync()
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<RendezvousDto>>("api/rendezvous") ?? new List<RendezvousDto>();
        }

        public async Task<RendezvousDto?> GetRendezvousAsync(int id)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<RendezvousDto>($"api/rendezvous/{id}");
        }

        public async Task<RendezvousDto> CreateAsync(object dto)
        {
            await EnsureAuthHeaderAsync();
            var resp = await _http.PostAsJsonAsync("api/rendezvous", dto);
            var content = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
            {
                // try to parse message and details properties from backend
                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;
                    string message = root.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() ?? "" : "Request failed";
                    string details = root.TryGetProperty("details", out var d) && d.ValueKind == JsonValueKind.String ? d.GetString() ?? "" : string.Empty;
                    var combined = string.IsNullOrWhiteSpace(details) ? message : message + " - " + details;
                    throw new HttpRequestException(combined);
                }
                catch (JsonException)
                {
                    throw new HttpRequestException(content);
                }
            }

            try
            {
                // Try to deserialize response into DTO with case-insensitive property matching
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dtoObj = System.Text.Json.JsonSerializer.Deserialize<RendezvousDto>(content, options);
                if (dtoObj != null) return dtoObj;

                // Fallback: tolerant manual parsing using TryGetProperty with multiple casings
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;
                int TryGetInt(string[] names)
                {
                    foreach (var n in names)
                    {
                        if (root.TryGetProperty(n, out var el) && el.ValueKind == JsonValueKind.Number)
                            return el.GetInt32();
                    }
                    return 0;
                }
                string TryGetString(string[] names)
                {
                    foreach (var n in names)
                    {
                        if (root.TryGetProperty(n, out var el) && el.ValueKind == JsonValueKind.String)
                            return el.GetString() ?? string.Empty;
                    }
                    return string.Empty;
                }

                var r = new RendezvousDto
                {
                    Id = TryGetInt(new[] { "Id", "id" }),
                    PatientId = TryGetInt(new[] { "PatientId", "patientId", "patientid" }),
                    MedecinId = TryGetInt(new[] { "MedecinId", "medecinId", "medecinid" }),
                    Date = TryGetString(new[] { "Date", "date" }),
                    Heure = TryGetString(new[] { "Heure", "heure" }),
                    Lieu = root.TryGetProperty("Lieu", out var l) ? l.GetString() : null,
                    Statut = root.TryGetProperty("Statut", out var s) ? s.GetString() : null
                };
                if (r.Id <= 0) throw new Exception("Missing Id in response");
                return r;
            }
            catch (HttpRequestException) { throw; }
            catch (Exception ex)
            {
                throw new HttpRequestException("Invalid response from server: " + ex.Message);
            }
        }
    }
}
