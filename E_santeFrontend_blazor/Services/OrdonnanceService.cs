using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class OrdonnanceService
    {
        private readonly HttpClient _http;
        private readonly E_santeFrontend.Helpers.LocalStorageService _storage;

        public OrdonnanceService(HttpClient http, E_santeFrontend.Helpers.LocalStorageService storage)
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

        public async Task<List<OrdonnanceDto>> GetOrdonnancesAsync()
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<OrdonnanceDto>>("api/ordonnance") ?? new List<OrdonnanceDto>();
        }

        public async Task<OrdonnanceDto?> GetOrdonnanceAsync(int id)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<OrdonnanceDto>($"api/ordonnance/{id}");
        }

        public async Task<bool> UpdateStatutAsync(int id, string statut)
        {
            await EnsureAuthHeaderAsync();
            var resp = await _http.PatchAsync($"api/ordonnance/{id}/statut", new StringContent($"\"{statut}\"", System.Text.Encoding.UTF8, "application/json"));
            return resp.IsSuccessStatusCode;
        }

        public async Task<OrdonnanceDto?> CreateAsync(object dto)
        {
            await EnsureAuthHeaderAsync();
            var resp = await _http.PostAsJsonAsync("api/ordonnance", dto);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadFromJsonAsync<OrdonnanceDto>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureAuthHeaderAsync();
            var resp = await _http.DeleteAsync($"api/ordonnance/{id}");
            return resp.IsSuccessStatusCode;
        }
    }
}
