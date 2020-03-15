using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candles.Client.Models.Candles;

namespace Candles.Client.Api
{
    /// <summary>
    /// Provides methods for work with candles API.
    /// </summary>
    public interface ICandlesApi
    {
        /// <summary>
        /// Returns all order books.
        /// </summary>
        /// <returns>A collection of order books.</returns>
        Task<IReadOnlyList<CandleModel>> GetAsync(string assetPairId, CandleType candleType, DateTime startDate,
            DateTime endDate);
    }
}
