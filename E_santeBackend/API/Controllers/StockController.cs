using E_santeBackend.Application.DTOs.Stock;
using E_santeBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("mouvement")]
        public async Task<IActionResult> AjouterMouvement([FromBody] StockMouvementCreateDto dto)
        {
            var result = await _stockService.AjouterMouvementAsync(dto);
            if (!result)
                return BadRequest(new { message = "Stock insuffisant ou médicament introuvable" });

            return Ok(new { message = "Mouvement ajouté avec succès" });
        }

        [HttpGet("medicament/{medicamentId}")]
        public async Task<IActionResult> GetByMedicamentId(int medicamentId)
        {
            var mouvements = await _stockService.GetByMedicamentIdAsync(medicamentId);
            return Ok(mouvements);
        }
    }
}