using FinanceTracker.API.Models;

namespace FinanceTracker.API.Repositories.Interfaces
{
    public interface IWatchlistRepository
    {
        Task<List<WatchlistItem>> GetAllAsync(string userId);
        Task<WatchlistItem?> GetByIdAsync(int id, string userId);
        Task AddAsync(WatchlistItem item);
        Task SaveChangesAsync();
        void Remove(WatchlistItem item);
    }
}
