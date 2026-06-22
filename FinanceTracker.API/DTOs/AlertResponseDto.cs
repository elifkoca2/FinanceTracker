namespace FinanceTracker.API.DTOs
{
    public class AlertResponseDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal TriggerPrice { get; set; }   
        public decimal TargetPrice { get; set; }     
        public DateTime TriggeredAt { get; set; }
        public bool IsRead { get; set; }
    }
}
