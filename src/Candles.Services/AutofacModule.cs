using Autofac;
using Candles.Domain;
using Candles.Domain.Handlers;
using Candles.Domain.Services;

namespace Candles.Services
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CandlesCache>()
                .As<ICandlesCache>()
                .SingleInstance();

            builder.RegisterType<CandlesService>()
                .As<ICandlesService>()
                .SingleInstance();

            builder.RegisterType<PricesHandler>()
                .As<IPricesHandler>()
                .SingleInstance();
        }
    }
}
