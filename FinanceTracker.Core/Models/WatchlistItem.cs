namespace FinanceTracker.Core.Models
{
    public class WatchlistItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;   
        public string CompanyName { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal TargetPrice { get; set; }
        public AlertDirection AlertDirection { get; set; }   
        public bool AlertTriggered { get; set; } = false;    
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
   
}

