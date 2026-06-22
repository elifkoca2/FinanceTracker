using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;

namespace FinanceTracker.API.Services.Interfaces
{
    public interface IAlertService
    { 
        // Fiyat hedefe ulaştı mı kontrol eder, ulaştıysa Alert oluşturur
        Task CheckAndCreateAlertAsync(WatchlistItem item);
        Task<List<AlertResponseDto>> GetAllAlertsAsync(string userId);
        Task<bool> MarkAsReadAsync(int alertId, string userId);
    }
}
