namespace FinanceTracker.API.Models
{
    public class PriceAlert
    {
        public int Id { get; set; }
        public int WatchlistItemId { get; set; }
        public WatchlistItem WatchlistItem { get; set; } = null!;  
        public string Symbol { get; set; } = string.Empty;
        public decimal TriggerPrice { get; set; }   
        public decimal TargetPrice { get; set; }    
        public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;   
    }
}
