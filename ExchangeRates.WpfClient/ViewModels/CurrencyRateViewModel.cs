using ExchangeRates.Extensions.DTOs;
using ExchangeRates.WpfClient.Services.Interfaces;
using ExchangeRates.WpfClient.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

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
            var currencyRates = await _service.GetCurrency(CurrencyName, StartDate, EndDate);

            ChartSeries = new ISeries[]
            {
                new LineSeries<CurrencyRateDto>
                {
                    Name = $"Chart",
                    Fill = new SolidColorPaint(SKColors.MediumAquamarine),
                    Stroke = new SolidColorPaint(SKColors.Aquamarine) { StrokeThickness = 1 },
                    TooltipLabelFormatter = point => $"{point.PrimaryValue:F4} BYN",
                    Values = currencyRates,
                    LineSmoothness = 0,
                    GeometrySize = 5,
                    GeometryFill = new SolidColorPaint(SKColors.WhiteSmoke),
                    GeometryStroke = new SolidColorPaint(SKColors.Aquamarine) {StrokeThickness = 4}
                }
            };

            XAxes = new List<Axis>()
            {
                new Axis
                {
                    Labels = currencyRates.Select(cr => cr.Date.ToShortDateString()).ToList(),
                    LabelsRotation = 45,
                    TextSize = 13,
                    MinLimit = 0,
                    MaxLimit = currencyRates.Count(),
                    MinStep = 1
                }
            };

            YAxes = new List<Axis>()
            {
                new Axis
                {
                    Labeler = (value) => value.ToString("F2")+" BYN",
                    TextSize = 13,
                    MinLimit = (double)currencyRates.Min(cr => cr.Value).Value - 0.01,
                    MaxLimit = (double)currencyRates.Max(cr => cr.Value).Value + 0.01,
                    MinStep = 0.005
                }
            };
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

        private ISeries[] _chartSeries;

        public ISeries[] ChartSeries
        {
            get { return _chartSeries; }
            set
            {
                if(_chartSeries == value)
                    return;
                _chartSeries = value;
                OnPropertyChanged(nameof(ChartSeries));
            }
        }

        private List<Axis> _xAxes;

        public List<Axis> XAxes
        {
            get { return _xAxes; }
            set
            {
                if (_xAxes == value)
                    return;
                _xAxes = value;
                OnPropertyChanged(nameof(XAxes));
            }
        }

        private List<Axis> _yAxes;

        public List<Axis> YAxes
        {
            get { return _yAxes; }
            set
            {
                if (_yAxes == value)
                    return;
                _yAxes = value;
                OnPropertyChanged(nameof(YAxes));
            }
        }
    }
}
