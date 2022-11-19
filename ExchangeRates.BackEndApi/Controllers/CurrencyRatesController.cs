using Microsoft.AspNetCore.Mvc;
using ExchangeRates.BackEndApi.Models;
using ExchangeRates.Extensions.SyncDataServices;
using AutoMapper;
using Newtonsoft.Json;
using ExchangeRates.Extensions.DTOs;
using ExchangeRates.BackEndApi.Repos.Interfaces;

namespace ExchangeRates.BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly IRatesRepo _currencyRatesRepo;
        private readonly IMapper _mapper;
        private readonly IHttpDataService _httpDataService;
        public CurrencyRatesController(IRatesRepo ratesRepo, IMapper mapper, IHttpDataService httpDataService)
        {
            _currencyRatesRepo = ratesRepo;
            _mapper = mapper;
            _httpDataService = httpDataService;
        }

        [HttpGet("")]
        public async Task<ICollection<CurrencyRateDto>> GetRatesAsync(string currency_Abbr, DateTime startDate, DateTime endDate)
        {
            ICollection<CurrencyRate> currencyRates = new List<CurrencyRate>();
            try
            {
                var date = startDate;
                do
                {
                    var currencyRate = _currencyRatesRepo.GetRateByAbbrOnDate(currency_Abbr, date);
                    if (currencyRate != null)
                    {
                        currencyRates.Add(currencyRate);
                        Console.WriteLine("!!!--> Rate has been loaded from cache <--!!!");
                    }
                    else
                    {
                        currencyRates.Add(await GetFromNbrbApiAsync(currency_Abbr,date));
                        Console.WriteLine("!!!--> Rate has been loaded from NBRB API <--!!!");
                    }
                    date = date.AddDays(1);
                }
                while (date <= endDate);
                await _currencyRatesRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!!--> We have a problem: {ex.Message} <--!!!");
            }
            return _mapper.Map<ICollection<CurrencyRateDto>>(currencyRates);
        }

        private async Task<CurrencyRate> GetFromNbrbApiAsync(string currency_Abbr, DateTime date)
        {
            var currencyRate = new CurrencyRate();
            try
            {
                Console.WriteLine("!!!--> Sending request to the NBRB Api <--!!!");
                var response = await _httpDataService.SendGetRequest
                (
                    $"https://www.nbrb.by/api/exrates/rates/{currency_Abbr}?parammode=2&ondate={date}"
                );
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    currencyRate = JsonConvert.DeserializeObject<CurrencyRate>(result);
                    _currencyRatesRepo.AddRate(currencyRate);
                    return currencyRate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!!--> Could not send synchronously: {ex.Message} <--!!!");
            }
            return currencyRate;
        }
    }
}