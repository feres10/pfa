using System.Net.Http.Json;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class ConsultationService
    {
        private readonly HttpClient _http;
        private readonly E_santeFrontend.Helpers.LocalStorageService _storage;

        public ConsultationService(HttpClient http, E_santeFrontend.Helpers.LocalStorageService storage)
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

        public async Task<List<ConsultationDto>> GetConsultationsByMedecinAsync(int medecinId)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<ConsultationDto>>($"api/consultation/medecin/{medecinId}") ?? new List<ConsultationDto>();
        }

        public async Task<List<ConsultationDto>> GetConsultationsTodayByMedecinAsync(int medecinId)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<ConsultationDto>>($"api/consultation/medecin/{medecinId}/today") ?? new List<ConsultationDto>();
        }

        public async Task<List<ConsultationDto>> GetConsultationsTodayForCurrentAsync()
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<ConsultationDto>>("api/consultation/me/today") ?? new List<ConsultationDto>();
        }
    }
}
