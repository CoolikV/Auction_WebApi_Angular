using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.BusinessLogic.Services
{
    public class TradeService : ITradeService
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }

        public TradeService(IUnitOfWork uow, IAdapter adapter)
        {
            Database = uow;
            Adapter = adapter;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public void StartTrade(int lotId)
        {
            var lot = Database.TradingLots.GetTradingLotById(lotId)
                ?? throw new NotFoundException($"Lot with id: {lotId}");

            if (IsTradeForLotAlreadyStarted(lotId))
                throw new AuctionException($"Trade for lot: {lot.Name} has already began");

            if (!lot.IsVerified)
                throw new AuctionException("Lot is not verified");

            Database.Trades.AddTrade(new Trade
            {
                LotId = lotId,
                TradingLot = lot,
                TradeStart = DateTime.Now,
                TradeEnd = DateTime.Now.AddDays(lot.TradeDuration)
            });

            Database.Save();
        }

        public void RateTradingLot(int tradeId, string userId, double price)
        {
            Trade trade = Database.Trades.GetTradeById(tradeId);
            UserProfile user = Database.Users.GetUserById(userId);

            if (trade == null || user == null)
                throw new ArgumentNullException();

            if (trade.TradingLot.User.Id == user.Id)
                throw new AuctionException("This is your lot");

            if (DateTime.Now.CompareTo(trade.TradeEnd) >= 0)
                throw new AuctionException("This trade is over");

            bool isNew = true;

            foreach (var el in user.Trades)
                if (el.Id == trade.Id)
                    isNew = false;

            if (trade.LastPrice < price)
            {
                trade.LastPrice = price;
                trade.LastRateUserId = userId;
                if (isNew)
                    user.Trades.Add(trade);
            }
            else
                throw new AuctionException($"Your price should be greater than: {trade.LastPrice}");

            Database.Users.UpdadeUser(user);
            Database.Trades.UpdadeTrade(trade);
            Database.Save();
        }

        public IEnumerable<TradeDTO> GetAllTrades()
        {
            return Adapter.Adapt<List<TradeDTO>>(Database.Trades.FindTrades());
        }

        public TradeDTO GetTradeById(int id)
        {
            var lot = Database.Trades.GetTradeById(id)
                ?? throw new NotFoundException($"Trade with id: {id}");

            return Adapter.Adapt<TradeDTO>(lot);
        }

        private bool IsTradeForLotAlreadyStarted(int lotId)
        {
            return Database.Trades.Entities.Any(t => t.LotId.Equals(lotId));
        }

        public TradeDTO GetTradeByLotId(int id)
        {
            var tradePoco = Database.Trades.FindTrades(t => t.LotId == id).FirstOrDefault();

            var a = Adapter.Adapt<TradeDTO>(tradePoco);
            return a;
        }

        public IEnumerable<TradeDTO> GetUserLoseTrades(string userId)
        {
            var user = Database.Users.GetUserById(userId);

            if (user == null)
                throw new ArgumentNullException();

            var list = user.Trades.Where(x => DateTime.Now.CompareTo(x.TradeEnd) >= 0 && x.LastRateUserId != user.Id);

            return Adapter.Adapt<List<TradeDTO>>(list);
        }

        public IEnumerable<TradeDTO> GetUserWinTrades(string userId)
        {
            var user = Database.Users.GetUserById(userId);

            if (user == null)
                throw new ArgumentNullException();

            var list = user.Trades.Where(x => DateTime.Now.CompareTo(x.TradeEnd) >= 0 && x.LastRateUserId == user.Id);

            return Adapter.Adapt<List<TradeDTO>>(list);
        }
    }
}