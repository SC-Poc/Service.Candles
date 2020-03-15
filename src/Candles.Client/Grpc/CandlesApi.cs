using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candles.Client.Api;
using Candles.Client.Models.Candles;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Service.Candles.Contracts;

namespace Candles.Client.Grpc
{
    internal class CandlesApi : ICandlesApi
    {
        private readonly Service.Candles.Contracts.Candles.CandlesClient _client;

        public CandlesApi(string address)
        {
            var channel = GrpcChannel.ForAddress(address);
            _client = new Service.Candles.Contracts.Candles.CandlesClient(channel);
        }

        public async Task<IReadOnlyList<CandleModel>> GetAsync(string assetPairId,
            Candles.Client.Models.Candles.CandleType candleType, DateTime startDate, DateTime endDate)
        {
            var response = await _client.GetAsync(new CandlesGetRequest
            {
                AssetPairId = assetPairId,
                Type = System.Enum.Parse<Service.Candles.Contracts.CandleType>(candleType.ToString()),
                StartDate = startDate.ToTimestamp(),
                EndDate = endDate.ToTimestamp()
            });

            return response.Candles
                .Select(candle => new CandleModel
                {
                    AssetPairId = candle.AssetPairId,
                    Type = System.Enum.Parse<Candles.Client.Models.Candles.CandleType>(candle.Type.ToString()),
                    Time = candle.Time.ToDateTime(),
                    Open = candle.Open,
                    Close = candle.Close,
                    High = candle.High,
                    Low = candle.Low
                })
                .ToList();
        }
    }
}
