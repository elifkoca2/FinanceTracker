using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Mock
{
    //Her hisse için durum 
    public class StockState
    {
        public decimal CurrentPrice { get; set; }
        public decimal Trend { get; set; }  // Yükseliş- Düşüş 
        public decimal Volatility { get; set; }  // Ne kadar sert hareket edeceği
        public int TrendDuration { get; set; }  // Trend kaç güncelleme devam eder 
        public int TrendCounter { get; set; }   // Kaç güncelleme 

    }

    public class StockSimulator
    {
        private static readonly Random _random = new Random();

        //Hisse başlangıç değerleri 
        private static readonly Dictionary<string, (decimal Price, decimal Volatility)> _initialValues = new()
        {
            { "AAPL",  (195m, 0.8m) },
            { "TSLA",  (250m, 2.0m) },
            { "MSFT",  (230m, 0.7m) },
            { "GOOGL", (175m, 0.9m) },
            { "AMZN",  (185m, 1.0m) },
            { "NVDA",  (480m, 2.5m) },
            { "META",  (320m, 1.2m) }
        };

        private static readonly Dictionary<string, StockState> _states = new();
       
        public static decimal GetNextPrice(string symbol)
        {
            symbol = symbol.ToUpper();

            if (!_states.ContainsKey(symbol))
            {
                _states[symbol] = CreateInitialState(symbol);
            }

            var state = _states[symbol];

            // Trend süresi dolunca yeni trend belirle
            if (state.TrendCounter >= state.TrendDuration)
            {
                RefreshTrend(state);
            }

            // Yeni fiyatı hesapla: trend + rastgele gürültü
            decimal trendEffect = state.Trend;
            decimal noise = (decimal)(_random.NextDouble() * 2 - 1) * state.Volatility;
            decimal changePercent = trendEffect + noise;

            // Fiyatı güncelle
            state.CurrentPrice *= (1 + changePercent / 100);
            state.TrendCounter++;

            // Fiyat çok aşırı olmaması için sınırla 
            var (basePrice, _) = GetInitialValues(symbol);
            state.CurrentPrice = Math.Max(state.CurrentPrice, basePrice * 0.5m);
            state.CurrentPrice = Math.Min(state.CurrentPrice, basePrice * 2.0m);

            return Math.Round(state.CurrentPrice, 2);
        }

        private static StockState CreateInitialState(string symbol) 
        { 
             var (price , volatility) = GetInitialValues(symbol);
            return new StockState
            {
                CurrentPrice = price,
                Volatility = volatility,
                Trend = GenerateRandomTrend(),
                TrendDuration = _random.Next(5, 15), // 5 ile 15 güncelleme arası 
                TrendCounter = 0
            };
        }

        private static void RefreshTrend(StockState state)
        {
            state.Trend = GenerateRandomTrend();
            state.TrendDuration = _random.Next(5, 15);
            state.TrendCounter = 0;
        }

        private static decimal GenerateRandomTrend()
         => (decimal)(_random.NextDouble() * 1.0 - 0.5);

        private static (decimal Price, decimal Volatility) GetInitialValues(string symbol)
            => _initialValues.TryGetValue(symbol, out var values)
             ? values
            : (100m, 1.0m);

    }
}
