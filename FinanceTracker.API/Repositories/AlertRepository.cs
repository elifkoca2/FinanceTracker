using FinanceTracker.API.Data;
using FinanceTracker.API.Models;
using FinanceTracker.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Repositories
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
