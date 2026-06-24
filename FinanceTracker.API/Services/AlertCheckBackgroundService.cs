using FinanceTracker.API.Hubs;
using FinanceTracker.Core.Interfaces.Repositories;
using FinanceTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace FinanceTracker.API.Services
{
    public class AlertCheckBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<PriceHub> _hubContext;
        private readonly ILogger<AlertCheckBackgroundService> _logger;

        // Alert kontrolü fiyat güncellemesinden biraz sonra çalışsın önce fiyat güncellensin, sonra kontrol etsin
        private const int CheckIntervalSeconds = 35;

        public AlertCheckBackgroundService(IServiceScopeFactory scopeFactory, IHubContext<PriceHub> hubContext, ILogger<AlertCheckBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Alert kontrol servisi başladı");
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAllAlertsAsync();
                await Task.Delay(TimeSpan.FromSeconds(CheckIntervalSeconds), stoppingToken);
            }
            _logger.LogInformation("Alert servisi durduruldu.");
        }

        private async Task CheckAllAlertsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var watchlistRepository = scope.ServiceProvider.GetRequiredService<IWatchlistRepository>();
            var alertService = scope.ServiceProvider.GetRequiredService<IAlertService>();

            try
            {
                var allItems = await watchlistRepository.GetAllAsync();
                foreach (var item in allItems)
                {
                    var wasTriggered = item.AlertTriggered;

                    //Alert kontrolü - hedef aşıldı mı 
                    await alertService.CheckAndCreateAlertAsync(item);

                    //Eğer alert tetiklendiyse SignalR ile bildir 
                    if (!wasTriggered && item.AlertTriggered)
                    {
                        _logger.LogInformation(
                            "{Symbol} hedefe ulaştı! Kullanıcı : {UserId}",
                            item.Symbol, item.UserId);

                        await _hubContext.Clients
                            .User(item.UserId)
                            .SendAsync("AlertTriggered", new
                            {
                                itemId = item.Id,
                                symbol = item.Symbol,
                                currentPrice = item.CurrentPrice,
                                targetPrice = item.TargetPrice
                            });
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Alert kontrol sırasında hata oluştu.");
            }
        }
    }
}
