using E_santeBackend.Application.DTOs.Auth;
using E_santeBackend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace E_santeBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly E_santeBackend.Infrastructure.Data.EHealthDbContext _db;

        public AuthController(IAuthService authService, E_santeBackend.Infrastructure.Data.EHealthDbContext db)
        {
            _authService = authService;
            _db = db;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            
            if (token == null)
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            try
            {
                // Read raw request body to avoid automatic model-binding 400 and improve diagnostics
                string raw;
                using (var reader = new System.IO.StreamReader(Request.Body))
                {
                    raw = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(raw))
                    return BadRequest(new { message = "Payload vide ou manquant." });

                // Attempt to deserialize tolerant to property name casing
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Application.DTOs.Auth.RegisterDto? registerDto = null;
                try
                {
                    registerDto = System.Text.Json.JsonSerializer.Deserialize<Application.DTOs.Auth.RegisterDto>(raw, options)
                                   ?? new Application.DTOs.Auth.RegisterDto();
                }
                catch (System.Text.Json.JsonException jex)
                {
                    return BadRequest(new { message = "Payload JSON invalide", detail = jex.Message, raw = raw });
                }

                if (registerDto == null)
                    return BadRequest(new { message = "Impossible de parser le payload.", raw = raw });

                // Basic server-side validation before calling the service
                if (string.IsNullOrWhiteSpace(registerDto.Email)
                    || string.IsNullOrWhiteSpace(registerDto.Password)
                    || string.IsNullOrWhiteSpace(registerDto.Nom)
                    || string.IsNullOrWhiteSpace(registerDto.Prenom))
                {
                    return BadRequest(new { message = "Champs requis manquants: nom, prénom, email ou mot de passe.", payload = registerDto, raw = raw });
                }

                var email = (registerDto.Email ?? string.Empty).Trim();
                var existing = await _db.Comptes.FirstOrDefaultAsync(c => c.Email == email);
                if (existing != null)
                {
                    return BadRequest(new { message = "Un compte avec cet email existe déjà.", payload = registerDto });
                }

                // Force role to Patient for public registration
                registerDto.Role = "Patient";

                var result = await _authService.RegisterAsync(registerDto);
                if (!result)
                    return BadRequest(new { message = "L'inscription a échoué. (voir logs serveur pour détails)", payload = registerDto, raw = raw });

                return Ok(new { message = "Inscription réussie" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur lors de l'inscription", detail = ex.Message });
            }
        }
    }
}