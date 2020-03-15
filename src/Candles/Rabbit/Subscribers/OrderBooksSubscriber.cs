using System;
using System.Linq;
using System.Threading.Tasks;
using Candles.Configuration.Service;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using MatchingEngine.Client.Contracts.Outgoing;
using Microsoft.Extensions.Logging;
using Candles.Configuration.Service.Rabbit.Subscribers;
using Candles.Domain.Handlers;
using Swisschain.LykkeLog.Adapter;

namespace Candles.Rabbit.Subscribers
{
    public class OrderBooksSubscriber : IDisposable
    {
        private readonly SubscriberSettings _settings;
        private readonly IPricesHandler _pricesHandler;
        private readonly PriceType _priceType;
        private readonly ILogger<OrderBooksSubscriber> _logger;

        private RabbitMqSubscriber<OrderBookSnapshotEvent> _subscriber;

        public OrderBooksSubscriber(
            SubscriberSettings settings,
            IPricesHandler pricesHandler,
            PriceType priceType,
            ILogger<OrderBooksSubscriber> logger)
        {
            _settings = settings;
            _pricesHandler = pricesHandler;
            _priceType = priceType;
            _logger = logger;
        }

        public void Start()
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _settings.ConnectionString,
                ExchangeName = _settings.Exchange,
                QueueName = $"{_settings.Exchange}.{_settings.QueueSuffix}",
                DeadLetterExchangeName = null,
                IsDurable = false
            };

            _subscriber = new RabbitMqSubscriber<OrderBookSnapshotEvent>(LegacyLykkeLogFactoryToConsole.Instance,
                    settings,
                    new ResilientErrorHandlingStrategy(LegacyLykkeLogFactoryToConsole.Instance, settings,
                        TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new GoogleProtobufMessageDeserializer<OrderBookSnapshotEvent>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        private async Task ProcessMessageAsync(OrderBookSnapshotEvent message)
        {
            if (message.OrderBook.IsBuy && _priceType == PriceType.Ask)
                return;
            
            if (!message.OrderBook.IsBuy && _priceType == PriceType.Bid)
                return;
                
            if(!message.OrderBook.Levels.Any())
                return;
            
            try
            {
                string priceValue;

                if (_priceType == PriceType.Ask)
                {
                    priceValue = message.OrderBook.Levels
                        .OrderBy(level => level.Price)
                        .First()
                        .Price;
                }
                else if (_priceType == PriceType.Bid)
                {
                    priceValue = message.OrderBook.Levels
                        .OrderByDescending(level => level.Price)
                        .First()
                        .Price;
                }
                else
                {
                    return;
                }

                var assetPairId = message.OrderBook.Asset;
                var time = message.OrderBook.Timestamp.ToDateTime();
                var price = decimal.Parse(priceValue);

                await _pricesHandler.HandleAsync(assetPairId, time, (double) price);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred during processing order book. {@Message}",
                    message);
            }
        }
    }
}
