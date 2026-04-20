using E_santeBackend.Application.DTOs.Paiement;
using E_santeBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaiementController : ControllerBase
    {
        private readonly IPaiementService _paiementService;

        public PaiementController(IPaiementService paiementService)
        {
            _paiementService = paiementService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaiementCreateDto dto)
        {
            var paiement = await _paiementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByPatientId), new { patientId = paiement.PatientId }, paiement);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var paiements = await _paiementService.GetByPatientIdAsync(patientId);
            return Ok(paiements);
        }
    }
}