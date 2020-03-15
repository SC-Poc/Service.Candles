using System;

namespace Candles.Domain.Entities
{
    /// <summary>
    /// Represents a candle.
    /// </summary>
    public class Candle
    {
        public Candle()
        {
        }

        public Candle(DateTime time, CandleType type, string assetPairId, double price)
        {
            Time = time;
            Type = type;
            AssetPairId = assetPairId;
            Open = price;
            Close = price;
            High = price;
            Low = price;
        }

        /// <summary>
        /// The type.
        /// </summary>
        public string AssetPairId { get; set; }

        /// <summary>
        /// The date and time of open price.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The type.
        /// </summary>
        public CandleType Type { get; set; }

        /// <summary>
        /// The open price of period.
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// The close price of period.
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// The high price of period.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// The low price of period.
        /// </summary>
        public double Low { get; set; }

        public void Update(double price)
        {
            Close = price;

            if (High < price)
                High = price;

            if (Low > price)
                Low = price;
        }
    }
}
