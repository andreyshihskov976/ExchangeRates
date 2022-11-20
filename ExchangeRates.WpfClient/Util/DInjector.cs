using ExchangeRates.WpfClient.ViewModels;
using Ninject;
using Ninject.Modules;

namespace ExchangeRates.WpfClient.Util
{
    public class DInjector
    {
        private IKernel kernel;

        public DInjector()
        {
            NinjectModule mainModule = new MainModule();
            kernel = new StandardKernel(mainModule);
        }

        public CurrencyRateViewModel GetMainVM()
        {
            return kernel.Get<CurrencyRateViewModel>();
        }
    }
}
