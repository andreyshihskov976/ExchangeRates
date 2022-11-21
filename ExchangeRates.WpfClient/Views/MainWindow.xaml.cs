using ExchangeRates.WpfClient.ViewModels;
using System;
using System.Windows;

namespace ExchangeRates.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(CurrencyRateViewModel currencyRateViewModel)
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            DataContext = currencyRateViewModel;            
        }
       
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartDatePicker.DisplayDateStart = new DateTime(2017, 7, 1);
            StartDatePicker.DisplayDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            EndDatePicker.DisplayDateStart = new DateTime(2017, 7, 1);
            EndDatePicker.DisplayDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            EndDatePicker.SelectedDate = EndDatePicker.DisplayDateEnd;
        }
    }
}
