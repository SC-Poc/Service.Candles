using Autofac;
using Candles.Configuration;
using Candles.Managers;
using Candles.Rabbit.Subscribers;

namespace Candles
{
    public class AutofacModule : Module
    {
        private readonly AppConfig _config;

        public AutofacModule(AppConfig config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartupManager>()
                .SingleInstance();

            builder.RegisterType<OrderBooksSubscriber>()
                .WithParameter("settings", _config.CandlesService.Rabbit.Subscribers.OrderBooks)
                .WithParameter("priceType", _config.CandlesService.PriceType)
                .SingleInstance();
        }
    }
}
