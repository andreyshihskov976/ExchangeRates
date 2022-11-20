using AutoMapper;

namespace ExchangeRates.BackEndApi.MapperProfiles
{
    internal class AmountByIdConverter : IValueConverter<int, int>
    {
        public int Convert(int sourceMember, ResolutionContext context)
        {
            if(sourceMember == 298 || sourceMember == 456){
                return 100;
            }
            return 1;
        }
    }
}