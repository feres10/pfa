using System.Net.Http.Json;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class MedecinService
    {
        private readonly HttpClient _http;
        private readonly E_santeFrontend.Helpers.LocalStorageService _storage;

        public MedecinService(HttpClient http, E_santeFrontend.Helpers.LocalStorageService storage)
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

        public async Task<MedecinDto?> GetByIdAsync(int id)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<MedecinDto>($"api/Medecin/{id}");
        }

        public async Task<bool> UpdateAsync(int id, MedecinUpdateDto dto)
        {
            await EnsureAuthHeaderAsync();
            var resp = await _http.PutAsJsonAsync($"api/Medecin/{id}", dto);
            return resp.IsSuccessStatusCode;
        }
    }
}
