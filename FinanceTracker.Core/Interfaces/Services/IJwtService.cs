using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
        DateTime GetExpirationDate();
    }
}
