using FinanceTracker.API.Models;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.API.DTOs
{
    public class CreateWatchlistItemDto
    {
        [Required(ErrorMessage = "Sembol boş olamaz.")]
        [StringLength(10, MinimumLength = 1)]
        public string Symbol { get; set; } = string.Empty;

        [Range(0.01, 1000000, ErrorMessage = "Hedef fiyat geçerli bir değer olmalı.")]
        public decimal TargetPrice { get; set; }
       
        [EnumDataType(typeof(AlertDirection), ErrorMessage = "Geçersiz yön değeri.")]
        public AlertDirection AlertDirection { get; set; }
    }
}
