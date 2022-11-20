using ExchangeRates.BackEndApi.Models;
using ExchangeRates.BackEndApi.Repos.Interfaces;
using System.Text.Json;

namespace ExchangeRates.BackEndApi.Repos
{
    public class CurrencyRatesRepo : ICurrencyRatesRepo
    {
        private bool _hasBeenChanged = false;
        private ICollection<CurrencyRate> _rates;
        public ICollection<CurrencyRate> Rates
        {
            get { return _rates; }
            set { _rates = value; }
        }

        public CurrencyRatesRepo()
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

        public ICollection<CurrencyRate> GetCurrencyRates(int currency_Id, DateTime startDate, DateTime endDate)
        {
            return _rates.Where(r => r.Cur_ID == currency_Id && 
                (r.Date.Date >= startDate.Date && r.Date.Date <= endDate.Date))
                .ToList();
        }

        public void AddCurrencyRate(CurrencyRate rate)
        {
            if (rate != null)
            {
                _rates.Add(rate);
                _rates = _rates.OrderBy(r => r.Date).ToList();
                _hasBeenChanged = true;
            }
        }

        public void AddCurrencyRates(ICollection<CurrencyRate> rates)
        {
            if (rates != null && rates.Count != 0)
            {
                foreach (var r in rates)
                {
                    _rates.Add(r);
                }
                _rates = _rates.OrderBy(r => r.Date).ToList();
                _hasBeenChanged = true;
            }
        }

        public async Task SaveChangesAsync()
        {
            if (_hasBeenChanged)
                using (FileStream fs = new FileStream(AppContext.BaseDirectory + @"AppData/RatesCache.json", FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync<ICollection<CurrencyRate>>(fs, _rates);
                    Console.WriteLine("!!!--> Data has been saved to file <--!!!");
                }
            _hasBeenChanged = false;
        }
    }
}