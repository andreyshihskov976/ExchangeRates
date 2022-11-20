using AutoMapper;

namespace ExchangeRates.BackEndApi.MapperProfiles
{
    internal class IdToNameConverter : IValueConverter<int, string>
    {
        public string Convert(int sourceMember, ResolutionContext context)
        {
            if(sourceMember == 298 || sourceMember == 456){
                return "RUB";
            }
            if(sourceMember == 292 || sourceMember == 451){
                return "EUR";
            }
            if(sourceMember == 145 || sourceMember == 431){
                return "USD";
            }
            return "***";
        }
    }
}