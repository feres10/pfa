using E_santeBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicamentController : ControllerBase
    {
        private readonly IMedicamentService _medicamentService;

        public MedicamentController(IMedicamentService medicamentService)
        {
            _medicamentService = medicamentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicaments = await _medicamentService.GetAllAsync();
            return Ok(medicaments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medicament = await _medicamentService.GetByIdAsync(id);
            if (medicament == null)
                return NotFound();

            return Ok(medicament);
        }

        [HttpGet("expiring-soon")]
        public async Task<IActionResult> GetExpiringSoon()
        {
            var medicaments = await _medicamentService.GetExpiringSoonAsync();
            return Ok(medicaments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] E_santeBackend.Application.DTOs.Medicament.MedicamentCreateDto dto)
        {
            var created = await _medicamentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}