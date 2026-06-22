using FinanceTracker.API.Data;
using FinanceTracker.API.Models;
using FinanceTracker.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly AppDbContext _context;

        public WatchlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WatchlistItem item)
        => await _context.WatchlistItems.AddAsync(item);

        public async Task<List<WatchlistItem>> GetAllAsync(string userId)
           => await _context.WatchlistItems
            .Where(w => w.UserId == userId)   //  sadece bu kullanıcının kayıtlarını getirir
            .ToListAsync();

        public async Task<WatchlistItem?> GetByIdAsync(int id, string userId)
             => await _context.WatchlistItems
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

        public void Remove(WatchlistItem item)
          => _context.WatchlistItems.Remove(item);

        public async Task SaveChangesAsync()
         => await _context.SaveChangesAsync();
    }
}
