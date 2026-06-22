namespace FinanceTracker.API.Services.Interfaces
{
    public interface IPriceService
    {

        Task<decimal> GetCurrentPriceAsync(string symbol);
    }
}
