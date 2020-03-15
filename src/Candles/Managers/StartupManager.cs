using System.Threading.Tasks;
using Candles.Domain;
using Candles.Rabbit.Subscribers;

namespace Candles.Managers
{
    public class StartupManager
    {
        private readonly OrderBooksSubscriber _orderBooksSubscriber;
        private readonly ICandlesCache _candlesCache;

        public StartupManager(OrderBooksSubscriber orderBooksSubscriber, ICandlesCache candlesCache)
        {
            _orderBooksSubscriber = orderBooksSubscriber;
            _candlesCache = candlesCache;
        }

        public async Task StartAsync()
        {
            await _candlesCache.InitializeAsync();
            
            _orderBooksSubscriber.Start();
        }
    }
}
