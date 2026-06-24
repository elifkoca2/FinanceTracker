using FinanceTracker.API.Hubs;
using FinanceTracker.Core.Interfaces.Repositories;
using FinanceTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace FinanceTracker.API.Services
{
    public class PriceUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<PriceHub> _hubContext;
        private readonly ILogger<PriceUpdateBackgroundService> _logger;

        //30 sn fiyat güncelleme , Bu servis fiyat güncelliyor SignalR ile bildiriyor 
        private const int UpdateIntervalSeconds = 30;

        public PriceUpdateBackgroundService(IServiceScopeFactory scopeFactory, IHubContext<PriceHub> hubContext, ILogger<PriceUpdateBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Fiyat güncelleme servisi başladı.");
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateAllPriceAsync();
                await Task.Delay(TimeSpan.FromSeconds(UpdateIntervalSeconds), stoppingToken);
            }
            _logger.LogInformation("Fiyat güncelleme servisi durduruldu.");
        }

        private async Task UpdateAllPriceAsync()
        {
            //Backgroundservice singleton, Dbcontext scoped her çalışma da yeni bir scope oluşturuyoruz
            using var scope = _scopeFactory.CreateScope();
            var watchlistRepository = scope.ServiceProvider.GetRequiredService<IWatchlistRepository>();
            var priceService = scope.ServiceProvider.GetRequiredService<IPriceService>();

            try
            {
                var allItems = await watchlistRepository.GetAllAsync();
                foreach (var item in allItems)
                {
                    var oldPrice = item.CurrentPrice;
                    var newPrice = await priceService.GetCurrentPriceAsync(item.Symbol);

                    item.CurrentPrice = newPrice;
                    item.LastUpdated = DateTime.UtcNow;
                    await watchlistRepository.SaveChangesAsync();

                    _logger.LogInformation(
                        "{Symbol} fiyatı güncellendi: {Old} -> {New}",
                        item.Symbol, oldPrice, newPrice);
               

                    //Fiyat güncellemesini kullanıcıya signalr ile gönderme 
                    await _hubContext.Clients
                        .User(item.UserId)
                        .SendAsync("PriceUpdated", new
                        {
                            itemId = item.Id,
                            symbol = item.Symbol,
                            currentPrice = item.CurrentPrice,
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fiyat güncelleme sırasında hata oluştu");
            }

        }
    }
}