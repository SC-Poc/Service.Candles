using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Candles.Domain.Services;
using Grpc.Core;
using Service.Candles.Contracts;

namespace Candles.GrpcServices
{
    public class CandlesService : Service.Candles.Contracts.Candles.CandlesBase
    {
        private readonly ICandlesService _candlesService;
        private readonly IMapper _mapper;

        public CandlesService(ICandlesService candlesService, IMapper mapper)
        {
            _candlesService = candlesService;
            _mapper = mapper;
        }

        public override async Task<CandlesGetResponse> Get(CandlesGetRequest request, ServerCallContext context)
        {
            var startDate = request.StartDate.ToDateTime();
            var endDate = request.EndDate.ToDateTime();
            var type = Enum.Parse<Candles.Domain.Entities.CandleType>(request.Type.ToString());

            var candles = await _candlesService.GetAsync(request.AssetPairId, type, startDate, endDate);

            var response = new CandlesGetResponse();

            response.Candles.AddRange(_mapper.Map<List<Candle>>(candles));

            return response;
        }
    }
}
