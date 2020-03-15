using System;
using System.ComponentModel;
using Candles.Domain.Entities;

namespace Candles.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Trim(this DateTime value, CandleType candleType)
        {
            return candleType switch
            {
                CandleType.Minute => TrimToMinutes(value),
                CandleType.Hour => TrimToHours(value),
                CandleType.Day => TrimToDays(value),
                CandleType.Month => TrimToMonths(value),
                _ => throw new InvalidEnumArgumentException()
            };
        }

        public static DateTime TrimToSeconds(this DateTime value)
            => Trim(value, TimeSpan.TicksPerSecond);

        public static DateTime TrimToMinutes(this DateTime value)
            => Trim(value, TimeSpan.TicksPerMinute);

        public static DateTime TrimToHours(this DateTime value)
            => Trim(value, TimeSpan.TicksPerHour);

        public static DateTime TrimToDays(this DateTime value)
            => Trim(value, TimeSpan.TicksPerDay);

        public static DateTime TrimToMonths(this DateTime value)
            => new DateTime(value.Year, value.Month, 1, 0, 0, 0, value.Kind);

        private static DateTime Trim(DateTime date, long roundTicks)
            => new DateTime(date.Ticks - date.Ticks % roundTicks, date.Kind);
    }
}
