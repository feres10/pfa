using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class MedicamentService
    {
        private readonly HttpClient _http;
        private readonly E_santeFrontend.Helpers.LocalStorageService _storage;
        private readonly Microsoft.JSInterop.IJSRuntime _js;

        public string? LastError { get; private set; }

        public MedicamentService(HttpClient http, E_santeFrontend.Helpers.LocalStorageService storage, Microsoft.JSInterop.IJSRuntime js)
        {
            _http = http;
            _storage = storage;
            _js = js;
        }

        private async Task EnsureAuthHeaderAsync()
        {
            try
            {
                try
                {
                    var baseAddr = await _storage.GetItemAsync("auth_base");
                    if (!string.IsNullOrWhiteSpace(baseAddr))
                    {
                        try { _http.BaseAddress = new System.Uri(baseAddr); } catch { }
                    }
                }
                catch { }

                var token = await _storage.GetItemAsync("auth_token");
                try { await _js.InvokeAsync<object>("console.log", new object?[] { token }); } catch { }
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var t = token.Trim();
                    if (t.StartsWith("{") && t.Contains("token", System.StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(t);
                            if (doc.RootElement.TryGetProperty("token", out var v))
                            {
                                var clean = v.GetString();
                                if (!string.IsNullOrWhiteSpace(clean))
                                {
                                    token = clean;
                                    await _storage.SetItemAsync("auth_token", clean);
                                }
                            }
                        }
                        catch { }
                    }
                }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    try { await _js.InvokeAsync<object>("console.log", new object?[] { "[MedicamentService] Authorization header applied" }); } catch { }
                }
                else
                {
                    try { await _js.InvokeAsync<object>("console.warn", new object?[] { "[MedicamentService] no token found in localStorage" }); } catch { }
                }
            }
            catch (System.Exception ex)
            {
                try { await _js.InvokeAsync<object>("console.error", new object?[] { ex.Message }); } catch { }
            }
        }

        public async Task<List<MedicamentDto>> GetMedicamentsAsync()
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<MedicamentDto>>("api/medicament") ?? new List<MedicamentDto>();
        }

        public async Task<MedicamentDto?> CreateMedicamentAsync(MedicamentCreateDto dto)
        {
            LastError = null;
            await EnsureAuthHeaderAsync();
            var resp = await _http.PostAsJsonAsync("api/medicament", dto);
            if (!resp.IsSuccessStatusCode)
            {
                try
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    LastError = $"{(int)resp.StatusCode} {resp.ReasonPhrase}: {body}";
                }
                catch { LastError = $"{(int)resp.StatusCode} {resp.ReasonPhrase}"; }
                try { await _js.InvokeAsync<object>("console.error", new object?[] { LastError }); } catch { }
                return null;
            }
            try
            {
                return await resp.Content.ReadFromJsonAsync<MedicamentDto>();
            }
            catch (System.Exception ex)
            {
                LastError = ex.Message;
                return null;
            }
        }

        public async Task<MedicamentDto?> GetMedicamentAsync(int id)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<MedicamentDto>($"api/medicament/{id}");
        }
    }
}
