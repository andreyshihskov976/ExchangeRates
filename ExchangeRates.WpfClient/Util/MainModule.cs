using ExchangeRates.WpfClient.Services;
using ExchangeRates.WpfClient.Services.Interfaces;
using Ninject.Modules;

namespace ExchangeRates.WpfClient.Util
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICurrencyRateWebService>().To<CurrencyRateWebService>();
        }
    }
}
