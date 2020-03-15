using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candles.Domain.Entities;

namespace Candles.Domain.Services
{
    public interface ICandlesService
    {
        Task<IReadOnlyList<Candle>> GetAsync(string assetPairId, CandleType candleType, DateTime startDate,
            DateTime endDate);

        Task<Candle> GetLastAsync(string assetPairId, CandleType candleType);

        Task AddAsync(IReadOnlyList<Candle> candles);

        Task UpdateAsync(IReadOnlyList<Candle> candles);
    }
}
