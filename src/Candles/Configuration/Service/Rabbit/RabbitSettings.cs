using JetBrains.Annotations;
using Candles.Configuration.Service.Rabbit.Subscribers;

namespace Candles.Configuration.Service.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSettings
    {
        public RabbitSubscribers Subscribers { get; set; }
    }
}
