using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Candles.Domain;
using Candles.Domain.Entities;
using Candles.Domain.Repositories;

namespace Candles.Services
{
    public class CandlesCache : ICandlesCache
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly ICandlesRepository _candlesRepository;

        private readonly Dictionary<string, Dictionary<CandleType, CandlesBag>> _cache =
            new Dictionary<string, Dictionary<CandleType, CandlesBag>>();

        private bool _initialized;

        public CandlesCache(ICandlesRepository candlesRepository)
        {
            _candlesRepository = candlesRepository;
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            var candles = await _candlesRepository.GetAllAsync();

            foreach (var groupByAssetPairId in candles.GroupBy(candle => candle.AssetPairId))
            {
                var assetPairCandles = new Dictionary<CandleType, CandlesBag>();

                foreach (var groupByType in groupByAssetPairId.GroupBy(candle => candle.Type))
                {
                    var bag = new CandlesBag(groupByType.ToList());
                    assetPairCandles.Add(groupByType.Key, bag);
                }

                _cache.Add(groupByAssetPairId.Key, assetPairCandles);
            }

            _initialized = true;
        }

        public IReadOnlyList<Candle> GetAsync(string assetPairId, CandleType candleType, DateTime startDate,
            DateTime endDate)
        {
            _lock.EnterReadLock();

            try
            {
                if (!_cache.TryGetValue(assetPairId, out var candlesByAssetPairId))
                    return new List<Candle>();

                if (!candlesByAssetPairId.TryGetValue(candleType, out var bag))
                    return new List<Candle>();

                return bag.GetByRange(startDate, endDate);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public Candle GetLastAsync(string assetPairId, CandleType candleType)
        {
            _lock.EnterReadLock();

            try
            {
                if (!_cache.TryGetValue(assetPairId, out var candlesByAssetPairId))
                    return null;

                if (!candlesByAssetPairId.TryGetValue(candleType, out var bag))
                    return null;

                return bag.GetLast();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Set(IReadOnlyList<Candle> candles)
        {
            _lock.EnterWriteLock();

            try
            {
                foreach (var groupByAssetPairId in candles.GroupBy(candle => candle.AssetPairId))
                {
                    if (!_cache.TryGetValue(groupByAssetPairId.Key, out var candlesByAssetPairId))
                    {
                        candlesByAssetPairId = new Dictionary<CandleType, CandlesBag>();
                        _cache.Add(groupByAssetPairId.Key, candlesByAssetPairId);
                    }

                    foreach (var groupByType in groupByAssetPairId.GroupBy(candle => candle.Type))
                    {
                        if (!candlesByAssetPairId.TryGetValue(groupByType.Key, out var bag))
                        {
                            bag = new CandlesBag();
                            candlesByAssetPairId.Add(groupByType.Key, bag);
                        }

                        bag.Add(groupByType.ToList());
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
