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


        public async Task<PriceAlert?> GetByIdAsync(int id)
         => await _context.PriceAlerts.FindAsync(id);

        public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
    }
}
