namespace FinanceTracker.API.DTOs
{
    public class WatchlistItemResponseDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal TargetPrice { get; set; }
        public string AlertDirection { get; set; } = string.Empty; 
        public bool AlertTriggered { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
