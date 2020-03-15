using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candles.Domain;
using Candles.Domain.Entities;
using Candles.Domain.Repositories;
using Candles.Domain.Services;

namespace Candles.Services
{
    public class CandlesService : ICandlesService
    {
        private readonly ICandlesCache _candlesCache;
        private readonly ICandlesRepository _candlesRepository;

        public CandlesService(ICandlesCache candlesCache, ICandlesRepository candlesRepository)
        {
            _candlesCache = candlesCache;
            _candlesRepository = candlesRepository;
        }

        public Task<IReadOnlyList<Candle>> GetAsync(string assetPairId, CandleType candleType, DateTime startDate,
            DateTime endDate)
        {
            var candles = _candlesCache.GetAsync(assetPairId, candleType, startDate, endDate);

            return Task.FromResult(candles);
        }

        public Task<Candle> GetLastAsync(string assetPairId, CandleType candleType)
        {
            var candle = _candlesCache.GetLastAsync(assetPairId, candleType);

            return Task.FromResult(candle);
        }

        public async Task AddAsync(IReadOnlyList<Candle> candles)
        {
            _candlesCache.Set(candles);

            await _candlesRepository.InsertAsync(candles);
        }

        public async Task UpdateAsync(IReadOnlyList<Candle> candles)
        {
            _candlesCache.Set(candles);

            await _candlesRepository.UpdateAsync(candles);
        }
    }
}
