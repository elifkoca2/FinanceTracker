using FinanceTracker.Core.DTOs;
using FinanceTracker.Core.Models;
using FinanceTracker.Core.Interfaces.Repositories;
using FinanceTracker.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly ILogger<AlertService> _logger;

        public AlertService(IAlertRepository alertRepository, ILogger<AlertService> logger)
        {
            _alertRepository = alertRepository;
            _logger = logger;
        }

        public async Task CheckAndCreateAlertAsync(WatchlistItem item)
        {
            bool isTargetReached = IsTargetReached(item);

            if (isTargetReached && !item.AlertTriggered)
            {
                await CreateAlertAsync(item);
            }
            else if (!isTargetReached && item.AlertTriggered)
            {
                item.AlertTriggered = false;
                await _alertRepository.SaveChangesAsync();
                _logger.LogInformation("{Symbol} hedeften çıktı, alert sıfırlandı.", item.Symbol);
            }
        }

        public async Task<List<AlertResponseDto>> GetAllAlertsAsync(string userId)
        {
            var alerts = await _alertRepository.GetAllAsync(userId);
            _logger.LogInformation("Kullanıcı {UserId} bildirimleri getirildi. Toplam: {Count}", userId, alerts.Count);
            return alerts.Select(a => new AlertResponseDto
            {
                Id = a.Id,
                Symbol = a.Symbol,
                TriggerPrice = a.TriggerPrice,
                TargetPrice = a.TargetPrice,
                TriggeredAt = a.TriggeredAt,
                IsRead = a.IsRead
            }).ToList();
        }

        public async Task<bool> MarkAsReadAsync(int alertId, string userId)
        {
            var alert = await _alertRepository.GetByIdAsync(alertId);
            if (alert == null) return false;

            alert.IsRead = true;
            await _alertRepository.SaveChangesAsync();
            _logger.LogInformation("Alert {Id} okundu işaretlendi.", alertId);
            return true;
        }

        private static bool IsTargetReached(WatchlistItem item) =>
       item.AlertDirection == AlertDirection.Above
           ? item.CurrentPrice >= item.TargetPrice
           : item.CurrentPrice <= item.TargetPrice;

        private async Task CreateAlertAsync(WatchlistItem item)
        {
            var alert = new PriceAlert
            {
                WatchlistItemId = item.Id,
                Symbol = item.Symbol,
                TriggerPrice = item.CurrentPrice,
                TargetPrice = item.TargetPrice
            };

            await _alertRepository.AddAsync(alert);
            item.AlertTriggered = true;
            await _alertRepository.SaveChangesAsync();

            _logger.LogInformation(" Alert tetiklendi! {Symbol} - Fiyat: {Price}", item.Symbol, item.CurrentPrice);
        }
    }
}
