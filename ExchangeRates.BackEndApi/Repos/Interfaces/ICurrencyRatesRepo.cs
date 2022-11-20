using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRates.BackEndApi.Models;

namespace ExchangeRates.BackEndApi.Repos.Interfaces
{
    public interface ICurrencyRatesRepo
    {
        Task SaveChangesAsync();
        ICollection<CurrencyRate> GetCurrencyRates(int currency_Id, DateTime startDate, DateTime endDate);
        void AddCurrencyRate(CurrencyRate rate);
        void AddCurrencyRates(ICollection<CurrencyRate> rates);
    }
}