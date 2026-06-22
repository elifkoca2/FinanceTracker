using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using FinanceTracker.API.Repositories.Interfaces;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.API.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _repository;
        private readonly IPriceService _priceService;
        private readonly IAlertService _alertService;
        private readonly ILogger<WatchlistService> _logger;
        public WatchlistService(IWatchlistRepository repository, IPriceService priceService, IAlertService alertService, ILogger<WatchlistService> logger)
        {
            _repository = repository;
            _priceService = priceService;
            _alertService = alertService;
            _logger = logger;
        }

        public async Task<WatchlistItemResponseDto> AddAsync(CreateWatchlistItemDto dto, string userId)
        {
            var currentPrice = await _priceService.GetCurrentPriceAsync(dto.Symbol);

            var item = new WatchlistItem
            {
                UserId = userId,   //  kullanıcıya bağlandı
                Symbol = dto.Symbol.ToUpper(),
                TargetPrice = dto.TargetPrice,
                AlertDirection = dto.AlertDirection,
                CurrentPrice = currentPrice,
                LastUpdated = DateTime.UtcNow
            };

            await _repository.AddAsync(item);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Kullanıcı {UserId} hisse ekledi: {Symbol}", userId, item.Symbol);
            return MapToDto(item);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var item = await _repository.GetByIdAsync(id, userId);
            if (item == null)
            {
                _logger.LogWarning("Silme: Id {Id} bulunamadı.", id);
                return false;
            }

            _repository.Remove(item);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Kullanıcı {UserId} hisse sildi: {Symbol}", userId, item.Symbol);
            return true;
        }

        public async Task<List<WatchlistItemResponseDto>> GetAllAsync(string userId)
        {
            var items = await _repository.GetAllAsync(userId);
            _logger.LogInformation("Kullanıcı {UserId} takip listesi getirildi. Kayıt: {Count}", userId, items.Count);
            return items.Select(MapToDto).ToList();
        }

        public async Task<WatchlistItemResponseDto?> GetByIdAsync(int id, string userId)
        {
            var item = await _repository.GetByIdAsync(id, userId);
            if (item == null)
            {
                _logger.LogWarning("Id {Id} bulunamadı veya kullanıcıya ait değil.", id);
                return null;
            }
            return MapToDto(item);
        }

        public async Task<WatchlistItemResponseDto?> RefreshPriceAsync(int id, string userId)
        {
            var item = await _repository.GetByIdAsync(id, userId);
            if (item == null)
            {
                _logger.LogWarning("Refresh: Id {Id} bulunamadı.", id);
                return null;
            }

            var oldPrice = item.CurrentPrice;
            var newPrice = await _priceService.GetCurrentPriceAsync(item.Symbol);

            item.CurrentPrice = newPrice;
            item.LastUpdated = DateTime.UtcNow;
            await _repository.SaveChangesAsync();

            _logger.LogInformation("{Symbol} fiyatı güncellendi: {Old} -> {New}", item.Symbol, oldPrice, newPrice);
            await _alertService.CheckAndCreateAlertAsync(item);

            return MapToDto(item);
        }

        private static WatchlistItemResponseDto MapToDto(WatchlistItem item) => new()
        {
            Id = item.Id,
            Symbol = item.Symbol,
            CompanyName = item.CompanyName,
            CurrentPrice = item.CurrentPrice,
            TargetPrice = item.TargetPrice,
            AlertDirection = item.AlertDirection.ToString(),
            AlertTriggered = item.AlertTriggered,
            LastUpdated = item.LastUpdated
        };
    }
    }
