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
        private readonly ILogger _logger;
        private readonly IRatesRepo _currencyRatesRepo;
        private readonly IMapper _mapper;
        private readonly IHttpDataService _httpDataService;
        public CurrencyRatesController(ILogger<CurrencyRatesController> logger, IRatesRepo ratesRepo, IMapper mapper, IHttpDataService httpDataService)
        {
            _logger = logger;
            _currencyRatesRepo = ratesRepo;
            _mapper = mapper;
            _httpDataService = httpDataService;
        }

        [HttpGet("")]
        public async Task<ICollection<CurrencyRateDto>> GetRatesAsync(string currency_Abbr, DateTime startDate, DateTime endDate)
        {
            _logger.Log(LogLevel.Information, $"{DateTime.Now} - Executing request with params: {currency_Abbr}, {startDate}, {endDate}.");
            ICollection<CurrencyRate> currencyRates = new List<CurrencyRate>();
            try
            {
                var date = startDate.Date;
                do
                {
                    var currencyRate = _currencyRatesRepo.GetRateByAbbrOnDate(currency_Abbr, date);
                    if (currencyRate != null)
                    {
                        currencyRates.Add(currencyRate);
                    }
                    else
                    {
                        _logger.Log(LogLevel.Warning, $"{DateTime.Now} - No currency rate found with the following properties: {currency_Abbr}, {date}.");
                        currencyRates.Add(await GetFromNbrbApiAsync(currency_Abbr,date));
                    }
                    date = date.AddDays(1);
                }
                while (date <= endDate.Date);
                await _currencyRatesRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"{DateTime.Now} - A error occured: {ex.Message}.");
            }
            _logger.Log(LogLevel.Information, $"Request completed successfully. The result of the request is sent to the client.");
            return _mapper.Map<ICollection<CurrencyRateDto>>(currencyRates);
        }

        private async Task<CurrencyRate> GetFromNbrbApiAsync(string currency_Abbr, DateTime date)
        {
            var currencyRate = new CurrencyRate();
            try
            {
                _logger.Log(LogLevel.Information, $"{DateTime.Now} - Sending a request to NBRB API with the following properties: {currency_Abbr}, {date}.");
                var response = await _httpDataService.SendGetRequest
                (
                    $"https://www.nbrb.by/api/exrates/rates/{currency_Abbr}?parammode=2&ondate={date}"
                );
                if (response.IsSuccessStatusCode)
                {
                    _logger.Log(LogLevel.Information, $"{DateTime.Now} - Response from NBRB API received successfully.");
                    var result = await response.Content.ReadAsStringAsync();
                    currencyRate = JsonConvert.DeserializeObject<CurrencyRate>(result);
                    _currencyRatesRepo.AddRate(currencyRate);
                    return currencyRate;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"{DateTime.Now} - A error occured: {ex.Message}.");
            }
            return currencyRate;
        }
    }
}