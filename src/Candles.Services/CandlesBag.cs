using System;
using System.Collections.Generic;
using System.Linq;
using Candles.Domain.Entities;
using Candles.Domain.Extensions;

namespace Candles.Services
{
    public class CandlesBag
    {
        private readonly SortedList<DateTime, Candle> _candles;

        public CandlesBag()
        {
            _candles = new SortedList<DateTime, Candle>();
        }

        public CandlesBag(IReadOnlyList<Candle> candles)
        {
            _candles = new SortedList<DateTime, Candle>(candles.ToDictionary(candle => candle.Time, candle => candle));
        }

        public IReadOnlyList<Candle> GetByRange(DateTime startDate, DateTime endDate)
        {
            var index = _candles.SearchEqualOrGreater(startDate);

            if (index == -1 || index >= _candles.Count)
                return Array.Empty<Candle>();

            var result = new List<Candle>();

            for (var i = index; i < _candles.Count; i++)
            {
                var candle = _candles.Values[i];

                if (candle.Time > endDate)
                    break;

                result.Add(candle);
            }

            return result;
        }

        public Candle GetLast()
        {
            if (!_candles.Any())
                return null;

            var key = _candles.Keys.Last();

            return _candles[key];
        }

        public void Add(IReadOnlyList<Candle> candles)
        {
            foreach (var candle in candles)
                _candles[candle.Time] = candle;
        }
    }
}
