using FinanceTracker.Core.DTOs;

namespace FinanceTracker.Core.Interfaces.Services
{
    public interface IWatchlistService
    {
        Task<List<WatchlistItemResponseDto>> GetAllAsync(string userId);
        Task<WatchlistItemResponseDto?> GetByIdAsync(int id, string userId);
        Task<WatchlistItemResponseDto> AddAsync(CreateWatchlistItemDto dto, string userId);
        Task<WatchlistItemResponseDto?> RefreshPriceAsync(int id, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
