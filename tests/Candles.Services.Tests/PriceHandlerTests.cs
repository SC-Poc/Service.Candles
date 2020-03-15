using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candles.Domain.Entities;
using Candles.Domain.Handlers;
using Candles.Domain.Services;
using Moq;
using Xunit;

namespace Candles.Services.Tests
{
    public class PriceHandlerTests
    {
        private readonly Mock<ICandlesService> _candleServiceMock = new Mock<ICandlesService>();

        private readonly IPricesHandler _handler;
        
        public PriceHandlerTests()
        {
            _handler = new PricesHandler(_candleServiceMock.Object);
        }
        
        [Fact]
        public async Task Initial_Update()
        {
            // arrange

            var assetPairId = "BTCUSD";
            var time = DateTime.UtcNow;
            var price = 100.1d;

            // act

            await _handler.HandleAsync(assetPairId, time, price);

            // assert

            _candleServiceMock.Verify(o => o.AddAsync(It.IsAny<List<Candle>>()), Times.Once);
        }
    }
}
