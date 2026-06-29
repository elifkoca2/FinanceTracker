using FinanceTracker.Core.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.Core.Interfaces.Services
{
    public interface IAlertService
    { 
        // Fiyat hedefe ulaştı mı kontrol eder, ulaştıysa Alert oluşturur
        Task CheckAndCreateAlertAsync(WatchlistItem item);
        Task<List<AlertResponseDto>> GetAllAlertsAsync(string userId);
        Task MarkAllAsReadAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}
