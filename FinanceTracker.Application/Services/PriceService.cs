using FinanceTracker.Core.Interfaces.Services;

namespace FinanceTracker.Application.Services
{
    public class PriceService : IPriceService
    {
        private static readonly Random _random = new();
        public Task<decimal> GetCurrentPriceAsync(string symbol)
        {
          //  throw new Exception("Test amaçlı kasıtlı hata");

            decimal price = _random.Next(100, 400) + (decimal)_random.NextDouble();
            return Task.FromResult(Math.Round(price, 2));
        }
    }
}
