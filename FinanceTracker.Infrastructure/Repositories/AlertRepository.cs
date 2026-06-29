using FinanceTracker.Infrastructure.Data;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly AppDbContext _context;
        public AlertRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PriceAlert alert)
         => await _context.PriceAlerts.AddAsync(alert);


        public async Task<List<PriceAlert>> GetAllAsync(string userId)
            => await _context.PriceAlerts
            .Where(a => a.WatchlistItem.UserId == userId)   
            .OrderByDescending(a => a.TriggeredAt)
            .ToListAsync();


        public async Task<PriceAlert?> GetByIdAsync(int id,string userId)  
             => await _context.PriceAlerts
        .Include(a => a.WatchlistItem)
        .FirstOrDefaultAsync(a => a.Id == id && a.WatchlistItem.UserId == userId);
      

        public async Task<int> GetUnreadCountAsync(string userId)
        => await _context.PriceAlerts
            .Include(a => a.WatchlistItem)
            .Where(a => a.WatchlistItem.UserId == userId && !a.IsRead)
            .CountAsync();

        public async Task MarkAllAsReadAsync(string userId)
        {
            var unreadAlerts = await _context.PriceAlerts
                .Include(a => a.WatchlistItem)
                .Where(a => a.WatchlistItem.UserId == userId && !a.IsRead)
                .ToListAsync();

            foreach (var alert in unreadAlerts)
                alert.IsRead = true;
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
