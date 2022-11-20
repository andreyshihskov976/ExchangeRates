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
        private readonly ICurrencyRatesRepo _currencyRatesRepo;
        private readonly IMapper _mapper;
        private readonly IHttpDataService _httpDataService;
        public CurrencyRatesController(ICurrencyRatesRepo ratesRepo, IMapper mapper, IHttpDataService httpDataService)
        {
            _currencyRatesRepo = ratesRepo;
            _mapper = mapper;
            _httpDataService = httpDataService;
        }

        [HttpGet("")]
        public async Task<ICollection<CurrencyRateDto>> GetRateDynamicsAsync(int currency_Id, DateTime startDate, DateTime endDate)
        {
            var currencyRates = _currencyRatesRepo.GetCurrencyRates(currency_Id, startDate, endDate);
            if (currencyRates.Count == 0)
            {
                await AddMissingCurrencyRates(currency_Id, startDate, endDate);
                currencyRates = _currencyRatesRepo.GetCurrencyRates(currency_Id, startDate, endDate);
            }
            else
            {
                var missingStartDate = FindFirtsMissingDate(startDate, endDate, currencyRates);
                var missingEndDate = FindLastMissingDate(startDate, endDate, currencyRates);
                if (missingStartDate <= missingEndDate)
                {
                    await AddMissingCurrencyRates(currency_Id, missingStartDate, missingEndDate);
                    currencyRates = _currencyRatesRepo.GetCurrencyRates(currency_Id, startDate, endDate);
                }
            }
            return _mapper.Map<ICollection<CurrencyRateDto>>(currencyRates);
        }

        private static DateTime FindFirtsMissingDate(DateTime start, DateTime end, ICollection<CurrencyRate> currencyRates)
        {
            while (start < end)
                if (currencyRates.FirstOrDefault(r => r.Date.Date == start.Date) != null)
                    start = start.AddDays(1);
                else
                    break;
            return start;
        }

        private static DateTime FindLastMissingDate(DateTime start, DateTime end, ICollection<CurrencyRate> currencyRates)
        {
            while (end > start)
                if (currencyRates.FirstOrDefault(r => r.Date.Date == end.Date) != null)
                    end = end.AddDays(-1);
                else
                    break;
            return end;
        }

        private async Task AddMissingCurrencyRates(int currency_Id, DateTime startDate, DateTime endDate)
        {
            var missingCurrencyRates = await GetFromNbrbApiAsync(currency_Id, startDate, endDate);
            _currencyRatesRepo.AddCurrencyRates(missingCurrencyRates);
            await _currencyRatesRepo.SaveChangesAsync();
        }

        private async Task<ICollection<CurrencyRate>> GetFromNbrbApiAsync(int currency_Id, DateTime startDate, DateTime endDate)
        {
            try
            {
                Console.WriteLine("!!!--> Sending request to the NBRB Api <--!!!");
                var response = await _httpDataService.SendGetRequest
                (
                    $"https://www.nbrb.by/API/ExRates/Rates/Dynamics/{currency_Id}?startDate={startDate}&endDate={endDate}"
                );
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var currencyRates = JsonConvert.DeserializeObject<ICollection<CurrencyRate>>(result);
                    return currencyRates;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!!--> Could not send synchronously: {ex.Message} <--!!!");
            }
            return null;
        }
    }
}