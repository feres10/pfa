using E_santeBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
        {
            var result = await _notificationService.CreateAsync(dto.CompteId, dto.Message);
            if (!result)
                return BadRequest();

            return Ok(new { message = "Notification créée avec succès" });
        }

        [HttpGet("compte/{compteId}")]
        public async Task<IActionResult> GetByCompteId(int compteId)
        {
            var notifications = await _notificationService.GetByCompteIdAsync(compteId);
            return Ok(notifications);
        }

        [HttpPatch("{id}/marquer-lue")]
        public async Task<IActionResult> MarquerCommeLue(int id)
        {
            var result = await _notificationService.MarquerCommeLueAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    public class CreateNotificationDto
    {
        public int CompteId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}