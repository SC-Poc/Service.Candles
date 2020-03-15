using JetBrains.Annotations;

namespace Candles.Configuration.Service.Rabbit.Subscribers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSubscribers
    {
        public SubscriberSettings OrderBooks { get; set; }
    }
}
