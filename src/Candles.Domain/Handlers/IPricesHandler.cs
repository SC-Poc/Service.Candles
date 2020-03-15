using System;
using System.Threading.Tasks;

namespace Candles.Domain.Handlers
{
    public interface IPricesHandler
    {
        Task HandleAsync(string assetPairId, DateTime time, double price);
    }
}
