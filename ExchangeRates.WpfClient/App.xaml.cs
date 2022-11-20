using ExchangeRates.WpfClient.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExchangeRates.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var injector = new DInjector();
            MainWindow = new MainWindow(injector.GetMainVM());
            MainWindow.Show();
        }
    }
}
