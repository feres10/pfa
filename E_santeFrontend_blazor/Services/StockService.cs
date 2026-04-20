using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using E_santeFrontend.Models;

namespace E_santeFrontend.Services
{
    public class StockService
    {
        private readonly HttpClient _http;
        public StockService(HttpClient http) { _http = http; }
        public Task<List<StockMouvementDto>> GetMovementsAsync() => _http.GetFromJsonAsync<List<StockMouvementDto>>("api/stock/movements");

        public async Task<bool> AjouterMouvementAsync(object dto)
        {
            var resp = await _http.PostAsJsonAsync("api/stock/mouvement", dto);
            return resp.IsSuccessStatusCode;
        }
    }
}
