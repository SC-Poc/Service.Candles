using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candles.Domain.Entities;

namespace Candles.Domain
{
    public interface ICandlesCache
    {
        Task InitializeAsync();

        IReadOnlyList<Candle> GetAsync(string assetPairId, CandleType candleType, DateTime startDate,
            DateTime endDate);

        Candle GetLastAsync(string assetPairId, CandleType candleType);

        void Set(IReadOnlyList<Candle> candles);
    }
}
