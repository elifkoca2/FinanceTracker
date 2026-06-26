using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Interfaces.Repositories
{
    public interface IWatchlistRepository
    {
        Task<List<WatchlistItem>> GetAllAsync(string userId);
        Task<List<WatchlistItem>> GetAllAsync(); //Background servisi için 
        Task<WatchlistItem?> GetByIdAsync(int id, string userId);
        Task<WatchlistItem?> GetBySymbolAsync(string symbol, string userId); // Aynı hisseyi tekrar eklememek için 
        Task AddAsync(WatchlistItem item);
        Task SaveChangesAsync();
        void Remove(WatchlistItem item);
    }
}
