using E_santeBackend.Application.DTOs.Auth;

namespace E_santeBackend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);
    }
}