using FinanceTracker.API.DTOs;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
  
    }
}
