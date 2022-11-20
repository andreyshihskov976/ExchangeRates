using ExchangeRates.Extensions.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.WpfClient.Services.Interfaces
{
    public interface ICurrencyRateWebService
    {
        Task<ICollection<CurrencyRateDto>> GetCurrency(string currencyName, DateTime startDate, DateTime dateTime);
    }
}
