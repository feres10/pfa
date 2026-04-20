using E_santeBackend.Infrastructure.Data;
using E_santeBackend.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly EHealthDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DebugController(EHealthDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin()
        {
            if (!_env.IsDevelopment())
                return Forbid();

            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Admin");
            if (adminRole == null) return BadRequest(new { message = "Role Admin introuvable" });

            var existing = await _context.Comptes.FirstOrDefaultAsync(c => c.Email == "admin@localhost");
            if (existing != null) return Ok(new { message = "Admin exists" });

            var compte = new E_santeBackend.Domain.Entities.CompteUtilisateur
            {
                Email = "admin@localhost",
                MotDePasse = PasswordHasher.HashPassword("Admin123!"),
                Actif = true,
                RoleId = adminRole.Id
            };

            _context.Comptes.Add(compte);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Admin created" });
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> ListAccounts()
        {
            if (!_env.IsDevelopment())
                return Forbid();

            var list = await _context.Comptes
                .Include(c => c.Role)
                .Select(c => new { c.Id, c.Email, Role = c.Role != null ? c.Role.Nom : null, c.Actif })
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!_env.IsDevelopment())
                return Forbid();

            var compte = await _context.Comptes.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (compte == null) return NotFound(new { message = "Compte introuvable" });

            compte.MotDePasse = E_santeBackend.Shared.Helpers.PasswordHasher.HashPassword(dto.NewPassword ?? "Password123!");
            await _context.SaveChangesAsync();
            return Ok(new { message = "Mot de passe mis à jour" });
        }

        public class ResetPasswordDto { public string Email { get; set; } = string.Empty; public string? NewPassword { get; set; } }

        [HttpGet("headers")]
        public IActionResult EchoHeaders()
        {
            if (!_env.IsDevelopment())
                return Forbid();

            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            return Ok(headers);
        }
    }
}
