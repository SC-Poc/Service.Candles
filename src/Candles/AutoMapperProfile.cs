using AutoMapper;
using Candles.Domain.Entities;
using Google.Protobuf.WellKnownTypes;

namespace Candles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Candle, Service.Candles.Contracts.Candle>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src =>
                    System.Enum.Parse<Service.Candles.Contracts.CandleType>(src.Type.ToString())))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.ToTimestamp()));

            CreateMap<Candle, Candles.Client.Models.Candles.CandleModel>(MemberList.Destination);
        }
    }
}
