using FinanceTracker.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Mock
{
    public class MockPriceService : IPriceService
    {
        public Task<decimal> GetCurrentPriceAsync(string symbol)
        {
            var price = StockSimulator.GetNextPrice(symbol);
            return Task.FromResult(price);
        }
    }
}
