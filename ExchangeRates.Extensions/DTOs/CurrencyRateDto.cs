using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRates.Extensions.DTOs
{
    public class CurrencyRateDto
    {
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public decimal? Value { get; set; }
        public int Amount { get; set; }
    }
}