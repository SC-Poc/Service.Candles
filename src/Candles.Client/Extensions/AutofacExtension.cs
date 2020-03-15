using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace Candles.Client.Extensions
{
    /// <summary>
    /// Extension for client registration.
    /// </summary>
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers <see cref="ICandlesClient"/> in Autofac container using <see cref="CandlesClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="settings">The client settings.</param>
        public static void RegisterCandlesClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] CandlesClientSettings settings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            builder.RegisterInstance(new CandlesClient(settings))
                .As<ICandlesClient>()
                .SingleInstance();
        }
    }
}
