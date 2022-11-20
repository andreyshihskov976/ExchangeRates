using ExchangeRates.Extensions.DTOs;
using ExchangeRates.WpfClient.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.WpfClient.Services
{
    public class CurrencyRateWebService : ICurrencyRateWebService
    {
        public async Task<ICollection<CurrencyRateDto>> GetCurrency(string currencyName, DateTime startDate, DateTime endDate)
        {
            using (var client = HttpClientService.CreateClient())
            {
                var response = await client.GetAsync
                (
                    $@"https://localhost:7195/api/CurrencyRates?currency_Id={currencyName}&startDate={startDate}&endDate={endDate}"
                );
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ICollection<CurrencyRateDto>>(result);
            }
        }
    }
}
