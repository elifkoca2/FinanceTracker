using FinanceTracker.API.Models;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
        DateTime GetExpirationDate();
    }
}
