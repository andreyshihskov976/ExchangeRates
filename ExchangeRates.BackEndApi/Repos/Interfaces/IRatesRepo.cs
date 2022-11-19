using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRates.BackEndApi.Models;

namespace ExchangeRates.BackEndApi.Repos.Interfaces
{
    public interface IRatesRepo
    {
        Task SaveChangesAsync();
        CurrencyRate GetRateByAbbrOnDate(string cur_Name, DateTime Date);
        void AddRate(CurrencyRate rate);
    }
}