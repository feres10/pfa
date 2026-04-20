using E_santeBackend.Application.DTOs.Ordonnance;
using E_santeBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdonnanceController : ControllerBase
    {
        private readonly IOrdonnanceService _ordonnanceService;

        public OrdonnanceController(IOrdonnanceService ordonnanceService)
        {
            _ordonnanceService = ordonnanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordonnances = await _ordonnanceService.GetAllAsync();
            return Ok(ordonnances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ordonnance = await _ordonnanceService.GetByIdAsync(id);
            if (ordonnance == null)
                return NotFound();

            return Ok(ordonnance);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrdonnanceCreateDto dto)
        {
            var ordonnance = await _ordonnanceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ordonnance.Id }, ordonnance);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ordonnanceService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/statut")]
        public async Task<IActionResult> UpdateStatut(int id, [FromBody] string statut)
        {
            var result = await _ordonnanceService.UpdateStatutAsync(id, statut);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}