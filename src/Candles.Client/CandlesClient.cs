using System;
using Candles.Client.Api;
using Candles.Client.Grpc;

namespace Candles.Client
{
    /// <inheritdoc />
    public class CandlesClient : ICandlesClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CandlesClient"/>.
        /// </summary>
        /// <param name="settings">The client settings.</param>
        public CandlesClient(CandlesClientSettings settings)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            
            Candles = new CandlesApi(settings.ServiceAddress);
        }
        
        /// <inheritdoc />
        public ICandlesApi Candles { get; }
    }
}
