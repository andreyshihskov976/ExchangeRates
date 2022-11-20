using ExchangeRates.Extensions.DTOs;
using ExchangeRates.WpfClient.Services.Interfaces;
using ExchangeRates.WpfClient.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ExchangeRates.WpfClient.ViewModels
{
    public class CurrencyRateViewModel : BaseViewModel
    {
        private ICurrencyRateWebService _service;

        public CurrencyRateViewModel(ICurrencyRateWebService service)
        {
            _service = service;
            GetChartCommand = new DelegateCommand(GetCharts);
            Currencies = new List<string>()
            {
                "RUB",
                "USD",
                "EUR"
            };
            CurrencyName = Currencies.First();
            this.PropertyChanged += CurrencyRateViewModel_PropertyChanged;
        }

        private void CurrencyRateViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StartDate) && StartDate > EndDate)
                EndDate = StartDate.AddDays(1);
        }

        private async void GetCharts(object obj)
        {
            CurrencyRates = await _service.GetCurrency(CurrencyName, StartDate, EndDate);
        }

        public ICommand GetChartCommand { get; private set; }

        public ICollection<string> Currencies { get; set; }
        
        private string _currencyName;

        public string CurrencyName
        {
            get { return _currencyName; }
            set 
            {
                if (_currencyName == value)
                    return;
                _currencyName = value;
                OnPropertyChanged(nameof(CurrencyName));
            }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set 
            {
                if (_startDate == value)
                    return;
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get { return _endDate; }
            set 
            {
                if (_endDate == value)
                    return;
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private ICollection<CurrencyRateDto> _currencyRates;

        public ICollection<CurrencyRateDto> CurrencyRates
        {
            get { return _currencyRates; }
            set 
            {
                if (_currencyRates == value)
                    return;
                _currencyRates = value;
                OnPropertyChanged(nameof(CurrencyRates));
            }
        }
    }
}
