using FinanceTracker.Core.DTOs;

namespace FinanceTracker.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
  
    }
}
