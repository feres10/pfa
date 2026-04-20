using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class NotificationService
    {
        private readonly HttpClient _http;
        private readonly E_santeFrontend.Helpers.LocalStorageService _storage;

        public NotificationService(HttpClient http, E_santeFrontend.Helpers.LocalStorageService storage)
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

        private static int? ExtractUserIdFromToken(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length >= 2)
                {
                    var payload = parts[1].Replace('-', '+').Replace('_', '/');
                    switch (payload.Length % 4)
                    {
                        case 2: payload += "=="; break;
                        case 3: payload += "="; break;
                    }
                    var bytes = System.Convert.FromBase64String(payload);
                    var json = System.Text.Encoding.UTF8.GetString(bytes);
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("sub", out var sub))
                    {
                        if (int.TryParse(sub.GetString(), out var id)) return id;
                    }
                    // also check common NameIdentifier claim
                    if (doc.RootElement.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", out var nid))
                    {
                        if (int.TryParse(nid.GetString(), out var id2)) return id2;
                    }
                }
            }
            catch { }
            return null;
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync()
        {
            await EnsureAuthHeaderAsync();
            try
            {
                var token = await _storage.GetItemAsync("auth_token");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var t = token.Trim();
                    if (t.StartsWith("{") && t.Contains("token", System.StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(t);
                            if (doc.RootElement.TryGetProperty("token", out var v))
                                t = v.GetString() ?? t;
                        }
                        catch { }
                    }
                    var userId = ExtractUserIdFromToken(t);
                    if (userId.HasValue)
                    {
                        return await _http.GetFromJsonAsync<List<NotificationDto>>($"api/notification/compte/{userId.Value}")
                               ?? new List<NotificationDto>();
                    }
                }
            }
            catch { }
            // Fallback: empty list if user id not available
            return new List<NotificationDto>();
        }
    }
}
