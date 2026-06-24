namespace FinanceTracker.Core.Interfaces.Services
{
    public interface IPriceService
    {

        Task<decimal> GetCurrentPriceAsync(string symbol);
    }
}
