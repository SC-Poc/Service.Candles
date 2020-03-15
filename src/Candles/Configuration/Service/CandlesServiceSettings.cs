using Candles.Configuration.Service.Db;
using Candles.Configuration.Service.Rabbit;

namespace Candles.Configuration.Service
{
    public class CandlesServiceSettings
    {
        public DbSettings Db { get; set; }

        public PriceType PriceType { get; set; }

        public RabbitSettings Rabbit { get; set; }
    }
}
