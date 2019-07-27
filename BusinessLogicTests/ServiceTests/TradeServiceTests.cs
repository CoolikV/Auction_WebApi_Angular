using Mapster;
using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Interfaces;
using Auction.BusinessLogic.Services;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Configs;
using System.Linq.Expressions;
using System.Linq;

namespace BusinessLogicTests.ServiceTests
{
    [TestFixture]
    class TradeServiceTests
    {
        static TradeServiceTests()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(BLMapRegister).Assembly);
        }

        private ITradeService tradeService;
        private Mock<IUnitOfWork> uow;
        private Mock<IGenericRepository<Trade>> tradeRepository;

        [SetUp]
        public void Load()
        {
            uow = new Mock<IUnitOfWork>();
            tradeRepository = new Mock<IGenericRepository<Trade>>();

            uow.Setup(x => x.Trades).Returns(tradeRepository.Object);
            uow.Setup(x => x.TradingLots.GetById(It.IsAny<int>())).Returns(new TradingLot { Name = It.IsAny<string>(), User = It.IsAny<User>(), TradeDuration = It.IsAny<int>(), Price = It.IsAny<double>(), IsVerified = true });
            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns(new User { Id = "defId" });

            tradeService = new TradeService(uow.Object);
        }

        [Test]
        public void StartTrade_TryToStartWithNullLot_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.TradingLots.GetById(It.IsAny<int>())).Returns<TradingLot>(null);

            //act & arrange
            Assert.Throws<ArgumentNullException>(() => tradeService.StartTrade(It.IsAny<int>()));
        }

        [Test]
        public void StartTrade_TryToStartTradeThatAlreadyBegun_ShouldThrowException()
        {
            //arrange
            tradeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Trade, bool>>>(),
                It.IsAny<Func<IQueryable<Trade>, IOrderedQueryable<Trade>>>(),
                It.IsAny<string>()))
                .Returns(new List<Trade> { new Trade { LastPrice = It.IsAny<double>() } });

            var lot = uow.Object.TradingLots.GetById(It.IsAny<int>());

            //act & arrange
            var ex = Assert.Throws<AuctionException>(() => tradeService.StartTrade(lot.Id));
            Assert.AreEqual(ex.Message, $"Trade for lot: {lot.Name} has already began");
        }

        [Test]
        public void StartTrade_TryToStartTradeWithNotVerifiedLot_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.TradingLots.GetById(It.IsAny<int>())).Returns(
                new TradingLot { Name = It.IsAny<string>(),
                    User = It.IsAny<User>(),
                    TradeDuration = It.IsAny<int>(),
                    Price = It.IsAny<double>(),
                    IsVerified = false });
            var lot = uow.Object.TradingLots.GetById(It.IsAny<int>());

            //act & assert
            var ex = Assert.Throws<AuctionException>(() => tradeService.StartTrade(lot.Id));
            Assert.AreEqual(ex.Message, "Lot is not verified");
        }

        [Test]
        public void StartTrade_TryToStartTrade_TradeRepositiryShouldCallsOnce()
        {
            //arrange
            var lot = uow.Object.TradingLots.GetById(It.IsAny<int>());

            //act
            tradeService.StartTrade(lot.Id);

            //assert
            tradeRepository.Verify(x => x.Insert(It.IsAny<Trade>()), Times.Once);
        }

        [Test]
        public void Rate_TryToRateWithNullUser_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns<User>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.RateTradingLot(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>()));
        }

        [Test]
        public void Rate_TryToRateWithNullTrade_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Trades.GetById(It.IsAny<int>())).Returns<TradingLot>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.RateTradingLot(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>()));
        }

        [Test]
        public void Rate_TryToRateOwnLot_ShouldThrowExcesption()
        {
            //arrange
            var lots = new List<TradingLot> { };
            var user = new User { Name = It.IsAny<string>(), TradingLots = lots };
            lots.Add(new TradingLot { Name = It.IsAny<string>(), User = user });
            var trade = new Trade { TradingLot = lots[0] };
            uow.Setup(x => x.Trades.GetById(It.IsAny<int>())).Returns(trade);
            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns(user);

            //act & arrange
            var ex = Assert.Throws<AuctionException>(() => tradeService.RateTradingLot(trade.Id, user.Id, It.IsAny<double>()));
            Assert.AreEqual("This is your lot", ex.Message);
        }

        [Test]
        public void Rate_TryToRateEndTrade_ShouldThrowException()
        {
            //arrange
            var trade = new Trade {
                TradingLot = new TradingLot { User = new User { } },
                TradeEnd = It.Is<DateTime>(x => x.CompareTo(DateTime.Now) > 0) };
            tradeRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(trade);

            //act & assert
            var ex = Assert.Throws<AuctionException>(() => tradeService.RateTradingLot(trade.Id, It.IsAny<string>(), It.IsAny<double>()));
            Assert.AreEqual(ex.Message, "This trade is over");

        }

        [Test]
        public void Rate_TryToRateWithSmallerPrice_ShouldThrowException()
        {
            //arrange
            var trade = new Trade {
                TradingLot = new TradingLot { User = new User { } },
                TradeEnd = DateTime.Now.AddDays(+3), LastPrice = It.IsAny<double>() };
            tradeRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(trade);

            //act & assert
            var ex = Assert.Throws<AuctionException>(() => tradeService.RateTradingLot(trade.Id, It.IsAny<string>(), It.Is<double>(x => trade.LastPrice > x)));//maybe create delegate for rate method and etc..
            Assert.AreEqual(ex.Message, $"Your price should be greater than: {trade.LastPrice}");
        }

        [Test]
        public void Rate_TryToRate_RepositoryShouldCallOnce()
        {
            //arrange
            var trade = new Trade { TradingLot = new TradingLot { User = new User { } }, TradeEnd = DateTime.Now.AddDays(+3), LastPrice = It.IsAny<double>() };
            tradeRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(trade);

            //act
            tradeService.RateTradingLot(trade.Id, It.IsAny<string>(), trade.LastPrice + 1);

            //assert
            tradeRepository.Verify(x => x.Update(It.IsAny<Trade>()), Times.Once);
        }

        [Test]
        public void GetUserWinTrades_TryToGetNonExistingUserWinTrades_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns<User>(null);


            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.GetUserWinTrades(It.IsAny<string>()));
        }

        [Test]
        public void GetUserLoseTrade_TryToGetNonExistingUserLoseTrades_ShouldThrowException()
        {
            //arrange
            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns<User>(null);

            //act & assert
            Assert.Throws<ArgumentNullException>(() => tradeService.GetUserLoseTrades(It.IsAny<string>()));
        }

        [Test]
        public void GetUserWinTrade_TryToGetActiveTrades()
        {
            //arrange
            var user = new User { Name = It.IsAny<string>() };
            var allTrades = new List<Trade> {
            new Trade {TradeEnd = DateTime.Now.AddDays(-1), LastRateUserId = It.Is<string>( x => x == user.Id)},
            new Trade { TradeEnd = DateTime.Now.AddDays(3)},
            new Trade { TradeEnd = DateTime.Now.AddDays(5)}};
            user.Trades = allTrades;

            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns(user);

            //act
            List<TradeDTO> list = tradeService.GetUserWinTrades(It.IsAny<string>()) as List<TradeDTO>;

            //assert
            Assert.AreEqual(list.Count, 1);
        }

        [Test]
        public void GetUserLoseTrade_TryToGetActiveTrades()
        {
            //arrange
            var user = new User { Id = "winId", Name = It.IsAny<string>() };
            var allTrades = new List<Trade> {
                new Trade {
                    TradeEnd = DateTime.Now.AddDays(-1),
                    LastRateUserId = It.Is<string>(x => x != user.Id)},
                new Trade { TradeEnd = DateTime.Now.AddDays(3)},
                new Trade { TradeEnd = DateTime.Now.AddDays(5)}};
            user.Trades = allTrades;

            uow.Setup(x => x.Users.GetById(It.IsAny<string>())).Returns(user);

            //act
            List<TradeDTO> list = tradeService.GetUserLoseTrades(It.IsAny<string>()) as List<TradeDTO>;

            //assert
            Assert.AreEqual(list.Count, 1);
        }
    }
}
