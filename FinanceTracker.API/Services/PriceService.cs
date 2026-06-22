using FinanceTracker.API.Services.Interfaces;

namespace FinanceTracker.API.Services
{
    public class PriceService : IPriceService
    {
        private static readonly Random _random = new();
        public Task<decimal> GetCurrentPriceAsync(string symbol)
        {
          //  throw new Exception("Test amaçlı kasıtlı hata");

            decimal price = _random.Next(100, 500) + (decimal)_random.NextDouble();
            return Task.FromResult(Math.Round(price, 2));
        }
    }
}
