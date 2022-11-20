using System.ComponentModel.DataAnnotations;

namespace ExchangeRates.BackEndApi.Models
{
    public class CurrencyRate
    {
        public int Cur_ID { get; set; }
        [Key]
        public DateTime Date { get; set; }
        public decimal? Cur_OfficialRate { get; set; }
    }
}