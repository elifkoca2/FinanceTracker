using FinanceTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Core.DTOs
{
    public class UpdateWatchlistItemDto
    {
        [Range(0.01, 10000,ErrorMessage ="Hedef fiyat 0'dan büyük olmalı.")]
        public decimal TargetPrice { get; set; }

        [EnumDataType(typeof(AlertDirection), ErrorMessage = "Geçersiz yön değeri.")]
        public AlertDirection AlertDirection { get; set; }


    }
}
