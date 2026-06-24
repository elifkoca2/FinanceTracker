using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Interfaces.Repositories
{
    public interface IAlertRepository
    {
        Task<List<PriceAlert>> GetAllAsync(string userId);
        Task<PriceAlert?> GetByIdAsync(int id);
        Task AddAsync(PriceAlert alert);
        Task SaveChangesAsync();
    }
}
