using AutoMapper;
using Candles.Domain.Entities;
using Candles.Repositories.Entities;

namespace Candles.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Candle, CandleEntity>(MemberList.Source);
            CreateMap<CandleEntity, Candle>(MemberList.Destination); 
        }
    }
}
