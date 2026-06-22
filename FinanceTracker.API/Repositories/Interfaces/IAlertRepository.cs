using FinanceTracker.API.Models;

namespace FinanceTracker.API.Repositories.Interfaces
{
    public interface IAlertRepository
    {
        Task<List<PriceAlert>> GetAllAsync(string userId);
        Task<PriceAlert?> GetByIdAsync(int id);
        Task AddAsync(PriceAlert alert);
        Task SaveChangesAsync();
    }
}
