using AutoMapper;
using ExchangeRates.BackEndApi.Models;
using ExchangeRates.Extensions.DTOs;

namespace ExchangeRates.BackEndApi.MapperProfiles
{
    public class CurrencyRateProfile : Profile
    {
        public CurrencyRateProfile()
        {
            //Source -> Target
            CreateMap<CurrencyRate, CurrencyRateDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Cur_Abbreviation))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Cur_Scale))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Cur_OfficialRate))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
        }
    }
}