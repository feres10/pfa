using System.Net.Http;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using E_santeFrontend.Helpers;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly LocalStorageService _storage;
        private readonly Microsoft.JSInterop.IJSRuntime _js;

        public string? LastError { get; private set; }
        public string? CurrentRole { get; private set; }

        private class TokenEnvelope { public string? Token { get; set; } public string? Message { get; set; } }

        public AuthService(HttpClient http, LocalStorageService storage, Microsoft.JSInterop.IJSRuntime js)
        {
            _http = http;
            _storage = storage;
            _js = js;
        }

        public async Task<bool> LoginAsync(LoginDto dto)
        {
            LastError = null;
            try
            {
                HttpResponseMessage resp;
                try
                {
                    resp = await _http.PostAsJsonAsync("api/auth/login", dto);
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    // Primary endpoint failed (connection refused, DNS, etc.). attempt fallback
                    resp = await TryFallbackLoginAsync(dto);
                    if (resp == null)
                    {
                        LastError = "Erreur de connexion au serveur. Vérifiez que l'API (" + (_http.BaseAddress?.ToString() ?? "") + ") est démarrée.";
                        return false;
                    }
                }

                // Ensure subsequent requests use the same scheme/authority that succeeded (https or http)
                try
                {
                    var usedUri = resp.RequestMessage?.RequestUri;
                    if (usedUri != null)
                    {
                        var authority = usedUri.GetLeftPart(System.UriPartial.Authority);
                        if (!string.IsNullOrWhiteSpace(authority))
                        {
                            _http.BaseAddress = new System.Uri(authority);
                            try { await _storage.SetItemAsync("auth_base", authority); } catch { }
                        }
                    }
                }
                catch { }
                if (!resp.IsSuccessStatusCode)
                {
                    // Try to read backend error { message: "..." }
                    try
                    {
                        var env = await resp.Content.ReadFromJsonAsync<TokenEnvelope>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        LastError = env?.Message;
                    }
                    catch
                    {
                        LastError = await resp.Content.ReadAsStringAsync();
                    }
                    return false;
                }

                // Try read plain string
                string? token = null;
                try
                {
                    token = await resp.Content.ReadFromJsonAsync<string>();
                }
                catch
                {
                    // Try read envelope { token: "..." }
                    try
                    {
                        var env = await resp.Content.ReadFromJsonAsync<TokenEnvelope>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        token = env?.Token;
                    }
                    catch
                    {
                        // Fallback: read raw content
                        var raw = await resp.Content.ReadAsStringAsync();
                        token = string.IsNullOrWhiteSpace(raw) ? null : raw.Trim('"');
                    }
                }

                // Normalize: if the value is a JSON object like {"token":"..."}, extract the token
                static string? ExtractToken(string? maybeJson)
                {
                    if (string.IsNullOrWhiteSpace(maybeJson)) return null;
                    var s = maybeJson.Trim();
                    if (s.StartsWith("{") && s.Contains("token", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using var doc = JsonDocument.Parse(s);
                            if (doc.RootElement.TryGetProperty("token", out var t))
                                return t.GetString();
                        }
                        catch { }
                    }
                    return s;
                }

                token = ExtractToken(token);

                if (string.IsNullOrWhiteSpace(token))
                {
                    LastError = "Token manquant dans la r�ponse.";
                    return false;
                }
                await _storage.SetItemAsync("auth_token", token);
                // Debug: log token storage
                try { await _js.InvokeAsync<object>("console.log", new object?[] { token }); } catch { }
                // Apply Authorization header to HttpClient for subsequent requests
                try
                {
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    try { await _js.InvokeAsync<object>("console.log", "[AuthService] Authorization header set on HttpClient"); } catch { }
                }
                catch { }
                // Decode JWT payload to extract role and store it
                try
                {
                    var parts = token.Split('.');
                    if (parts.Length >= 2)
                    {
                        string payload = parts[1];
                        // base64url -> base64
                        payload = payload.Replace('-', '+').Replace('_', '/');
                        switch (payload.Length % 4)
                        {
                            case 2: payload += "=="; break;
                            case 3: payload += "="; break;
                        }
                        var bytes = System.Convert.FromBase64String(payload);
                        var json = System.Text.Encoding.UTF8.GetString(bytes);
                        using var doc = JsonDocument.Parse(json);
                        // try common role claim keys; if missing, search for any property name that ends with 'role'
                        if (doc.RootElement.TryGetProperty("role", out var roleEl))
                        {
                            if (roleEl.ValueKind == JsonValueKind.Array && roleEl.GetArrayLength() > 0)
                                CurrentRole = roleEl[0].GetString();
                            else
                                CurrentRole = roleEl.GetString();
                        }
                        else if (doc.RootElement.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var r2))
                        {
                            if (r2.ValueKind == JsonValueKind.Array && r2.GetArrayLength() > 0)
                                CurrentRole = r2[0].GetString();
                            else
                                CurrentRole = r2.GetString();
                        }
                        else
                        {
                            foreach (var prop in doc.RootElement.EnumerateObject())
                            {
                                if (prop.Name.EndsWith("role", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (prop.Value.ValueKind == JsonValueKind.Array && prop.Value.GetArrayLength() > 0)
                                        CurrentRole = prop.Value[0].GetString();
                                    else
                                        CurrentRole = prop.Value.GetString();
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(CurrentRole))
                        {
                            await _storage.SetItemAsync("auth_role", CurrentRole);
                            try { await _js.InvokeAsync<object>("console.log", new object?[] { CurrentRole }); } catch { }
                        }

                        return true;
                    }
                }
                catch { /* ignore parse errors */ }
                return true;
            }
            catch
            {
                LastError = "Erreur de connexion au serveur. Vérifiez que l'API (https://localhost:7245) est démarrée.";
                return false;
            }
        }

        private async Task<HttpResponseMessage?> TryFallbackLoginAsync(LoginDto dto)
        {
            try
            {
                var baseAddr = _http.BaseAddress;
                if (baseAddr == null) return null;
                // Only attempt fallback for localhost scenarios
                if (!string.Equals(baseAddr.Host, "localhost", System.StringComparison.OrdinalIgnoreCase) && baseAddr.Host != "127.0.0.1")
                    return null;

                // Common dev HTTP port from launchSettings
                var altPort = baseAddr.Port;
                if (baseAddr.Scheme == "https" && (altPort == 7245 || altPort == 0))
                    altPort = 5139; // fallback http port used in launchSettings

                var builder = new System.UriBuilder(baseAddr)
                {
                    Scheme = "http",
                    Port = altPort
                };

                var authority = builder.Uri.GetLeftPart(System.UriPartial.Authority);
                try
                {
                    // Set HttpClient.BaseAddress to the fallback authority so subsequent requests use it
                    try { _http.BaseAddress = new System.Uri(authority); } catch { }
                    // Call the relative endpoint so the request uses the new BaseAddress
                    return await _http.PostAsJsonAsync("api/auth/login", dto);
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        // Ensure the token stored in localStorage is applied to the HttpClient's default headers.
        public async Task ApplyTokenToHttpClientAsync()
        {
            try
            {
                var token = await _storage.GetItemAsync("auth_token");
                // Normalize in case an old JSON-wrapped value is present
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var trimmed = token.Trim();
                    if (trimmed.StartsWith("{") && trimmed.Contains("token", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using var doc = JsonDocument.Parse(trimmed);
                            if (doc.RootElement.TryGetProperty("token", out var t))
                            {
                                var clean = t.GetString();
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
                    var baseAddr = await _storage.GetItemAsync("auth_base");
                    if (!string.IsNullOrWhiteSpace(baseAddr))
                    {
                        try { _http.BaseAddress = new System.Uri(baseAddr); } catch { }
                    }

                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    try { await _js.InvokeAsync<object>("console.log", new object?[] { "[AuthService] ApplyTokenToHttpClientAsync applied header" }); } catch { }
                }
                else
                {
                    try { await _js.InvokeAsync<object>("console.warn", new object?[] { "[AuthService] ApplyTokenToHttpClientAsync: no token found" }); } catch { }
                }
            }
            catch { }
        }

        public async Task LogoutAsync()
        {
            await _storage.RemoveItemAsync("auth_token");
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            LastError = null;
            try
            {
                var resp = await _http.PostAsJsonAsync("api/auth/register", dto);
                if (resp.IsSuccessStatusCode)
                    return true;

                // Try to read structured error from backend
                try
                {
                    var obj = await resp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (obj.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        if (obj.TryGetProperty("message", out var m) && m.ValueKind == System.Text.Json.JsonValueKind.String)
                        {
                            LastError = m.GetString();
                        }
                        else
                        {
                            LastError = obj.ToString();
                        }
                    }
                }
                catch
                {
                    LastError = await resp.Content.ReadAsStringAsync();
                }

                return false;
            }
            catch (System.Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }
    }
}
