using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace E_santeFrontend.Helpers
{
    public class JwtAuthHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public JwtAuthHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // Prefer synchronous in-process JS interop when available
                if (_jsRuntime is IJSInProcessRuntime inProc)
                {
                    var token = inProc.Invoke<string>("localStorage.getItem", "auth_token");
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
                                    if (!string.IsNullOrWhiteSpace(clean)) token = clean;
                                }
                            }
                            catch { }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                else
                {
                    // Fallback to async interop which works in non-in-process scenarios
                    try
                    {
                        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, "auth_token");
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
                                        if (!string.IsNullOrWhiteSpace(clean)) token = clean;
                                    }
                                }
                                catch { }
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                    }
                    catch
                    {
                        // ignore failures to read token
                    }
                }
            }
            catch
            {
                // ignore any JS interop issues and continue without auth header
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
