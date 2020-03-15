using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candles.Domain.Entities;
using Candles.Domain.Extensions;
using Candles.Domain.Handlers;
using Candles.Domain.Services;

namespace Candles.Services
{
    public class PricesHandler : IPricesHandler
    {
        private readonly ICandlesService _candlesService;

        private readonly Dictionary<CandleType, Candle> _candles = new Dictionary<CandleType, Candle>();
        private readonly CandleType[] _candleTypes;

        public PricesHandler(ICandlesService candlesService)
        {
            _candlesService = candlesService;

            _candleTypes = Enum
                .GetNames(typeof(CandleType))
                .Select(Enum.Parse<CandleType>)
                .Where(item => item != CandleType.None)
                .ToArray();

            foreach (var candleType in _candleTypes)
                _candles.Add(candleType, null);
        }

        public async Task HandleAsync(string assetPairId, DateTime time, double price)
        {
            var candleTypeTasks = _candleTypes
                .Select(candleType => HandleAsync(assetPairId, time, price, candleType))
                .ToList();

            await Task.WhenAll(candleTypeTasks);

            var newCandles = candleTypeTasks
                .Where(task => task.Result.Item1)
                .Select(task => task.Result.Item2)
                .ToList();

            var updatedCandles = candleTypeTasks
                .Where(task => !task.Result.Item1)
                .Select(task => task.Result.Item2)
                .ToList();

            var storeTasks = new List<Task>();

            if (newCandles.Any())
                storeTasks.Add(_candlesService.AddAsync(newCandles));

            if (updatedCandles.Any())
                storeTasks.Add(_candlesService.UpdateAsync(updatedCandles));

            await Task.WhenAll(storeTasks);
        }

        private async Task<(bool, Candle)> HandleAsync(string assetPairId, DateTime time, double price,
            CandleType candleType)
        {
            var candle = _candles[candleType];

            if (candle == null)
                candle = await _candlesService.GetLastAsync(assetPairId, candleType);

            var trimmedTime = time.Trim(candleType);

            if (candle == null || candle.Time != trimmedTime)
            {
                candle = new Candle(trimmedTime, candleType, assetPairId, price);
                _candles[candleType] = candle;

                return (true, candle);
            }

            candle.Update(price);
            _candles[candleType] = candle;

            return (false, candle);
        }
    }
}
