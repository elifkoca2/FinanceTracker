using FinanceTracker.API.DTOs;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
  
    }
}
