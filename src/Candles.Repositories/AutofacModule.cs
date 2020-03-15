using Autofac;
using Candles.Domain.Repositories;
using Candles.Repositories.Context;

namespace Candles.Repositories
{
    public class AutofacModule : Module
    {
        private readonly string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionFactory>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_connectionString))
                .SingleInstance();

            builder.RegisterType<CandlesRepository>()
                .As<ICandlesRepository>()
                .SingleInstance();
        }
    }
}
