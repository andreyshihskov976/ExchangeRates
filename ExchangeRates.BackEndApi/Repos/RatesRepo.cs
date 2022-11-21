using ExchangeRates.BackEndApi.Models;
using ExchangeRates.BackEndApi.Repos.Interfaces;
using System.Reflection;
using System.Text.Json;

namespace ExchangeRates.BackEndApi.Repos
{
    public class RatesRepo : IRatesRepo
    {
        private bool _hasBeenChanged = false;
        private ICollection<CurrencyRate> _rates;
        private readonly ILogger _logger;

        public ICollection<CurrencyRate> Rates
        {
            get { return _rates; }
            set { _rates = value; }
        }

        public RatesRepo(ILogger<RatesRepo> logger)
        {
            _logger = logger;
            LoadFromCache();
        }

        private void LoadFromCache()
        {
            try
            {
                using (FileStream fs = new FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\AppData\RatesCache.json", FileMode.Open))
                {
                    _rates = JsonSerializer.Deserialize<ICollection<CurrencyRate>>(fs);
                    _logger.Log(LogLevel.Information,$@"{DateTime.Now} - The cache was loaded successfully.");
                }
            }
            catch (FileNotFoundException)
            {
                _logger.Log(LogLevel.Critical, $@"{DateTime.Now} - The cache was not found.");
                _rates = new List<CurrencyRate>();
            }
        }

        public CurrencyRate GetRateByAbbrOnDate(string currency_Abbr, DateTime date)
        {
            return _rates.FirstOrDefault(r => r.Cur_Abbreviation == currency_Abbr && r.Date.Date == date.Date);
        }

        public async Task SaveChangesAsync()
        {
            if (_hasBeenChanged)
                using (FileStream fs = new FileStream(AppContext.BaseDirectory + @"AppData/RatesCache.json", FileMode.OpenOrCreate))
                {
                    _rates = _rates.OrderBy(r => r.Date).ToList();
                    await JsonSerializer.SerializeAsync(fs, _rates);
                    _logger.Log(LogLevel.Information, $@"{DateTime.Now} - The data was successfully cached.");
                }
            _hasBeenChanged = false;
        }

        public void AddRate(CurrencyRate rate)
        {
            if (rate != null)
            {
                _rates.Add(rate);
                _hasBeenChanged = true;
            }
        }
    }
}