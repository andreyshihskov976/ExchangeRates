using ExchangeRates.BackEndApi.Models;
using ExchangeRates.BackEndApi.Repos.Interfaces;
using System.Text.Json;

namespace ExchangeRates.BackEndApi.Repos
{
    public class RatesRepo : IRatesRepo
    {
        private bool _hasBeenChanged = false;
        private ICollection<CurrencyRate> _rates;
        public ICollection<CurrencyRate> Rates
        {
            get { return _rates; }
            set { _rates = value; }
        }

        public RatesRepo()
        {
            LoadFromCache();
        }

        private void LoadFromCache()
        {
            try
            {
                using (FileStream fs = new FileStream(AppContext.BaseDirectory + @"AppData/RatesCache.json", FileMode.Open))
                {
                    _rates = JsonSerializer.Deserialize<ICollection<CurrencyRate>>(fs);
                    Console.WriteLine("!!!--> Data has been loaded from file <--!!!");
                }
            }
            catch (FileNotFoundException)
            {
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
                    await JsonSerializer.SerializeAsync<ICollection<CurrencyRate>>(fs, _rates);
                    Console.WriteLine("!!!--> Data has been saved to file <--!!!");
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