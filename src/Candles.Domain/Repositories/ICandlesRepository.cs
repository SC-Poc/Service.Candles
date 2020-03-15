using System.Collections.Generic;
using System.Threading.Tasks;
using Candles.Domain.Entities;

namespace Candles.Domain.Repositories
{
    public interface ICandlesRepository
    {
        Task<IReadOnlyList<Candle>> GetAllAsync();
        Task InsertAsync(IReadOnlyList<Candle> candles);
        Task UpdateAsync(IReadOnlyList<Candle> candles);
    }
}
