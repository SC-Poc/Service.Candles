using Candles.Client.Api;

namespace Candles.Client
{
    /// <summary>
    /// Trade processing service client.
    /// </summary>
    public interface ICandlesClient
    {
        /// <summary>
        /// Candles API.
        /// </summary>
        ICandlesApi Candles { get; }
    }
}
