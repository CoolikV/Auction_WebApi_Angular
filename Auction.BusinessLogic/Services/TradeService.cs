using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Entities.Enums;
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
        ITradingLotService LotService { get; set; }

        public TradeService(IUnitOfWork uow, IAdapter adapter, ITradingLotService lotService)
        {
            Database = uow;
            Adapter = adapter;
            LotService = lotService;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
        //add methods to find trades and use it instead database.trades.gettdaebyid(int id) and etc...
        public void StartTrade(int lotId)
        {
            try
            {
                if (!LotService.IsLotExists(lotId))
                    throw new NotFoundException();
                if (IsTradeForLotAlreadyStarted(lotId))
                    throw new AuctionException($"Trade for this lot has already began");
                //maybe add method to get TradingLot from db in LotService
                var lot = Database.TradingLots.GetTradingLotById(lotId);

                if (lot.LotStatus == LotStatus.NotVerified)
                    throw new AuctionException("Lot is not verified");

                Database.Trades.AddTrade(new Trade
                {
                    LotId = lotId,
                    TradingLot = lot,
                    TradeStart = DateTime.Now,
                    TradeEnd = DateTime.Now.AddDays(lot.TradeDuration)
                });
            }
            catch(NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

            Database.Save();
        }

        public void RateTradingLot(int tradeId, string userId, double price)
        {
            Trade trade = Database.Trades.GetTradeById(tradeId)
                ?? throw new NotFoundException($"Trade with id: {tradeId}");

            UserProfile user = Database.UserProfiles.GetProfileById(userId) 
                ?? throw new NotFoundException($"User with id: {userId}");

            if (trade.TradingLot.User.Id == user.Id)
                throw new AuctionException("This is your lot");

            if (DateTime.Now.CompareTo(trade.TradeEnd) >= 0)
                throw new AuctionException("This trade is over");

            bool isNew = user.Trades.All(t => !t.Id.Equals(trade.Id));

            if (trade.LastPrice < price)
            {
                trade.LastPrice = price;
                trade.LastRateUserId = userId;
                if (isNew)
                    user.Trades.Add(trade);
            }
            else
                throw new AuctionException($"Your price should be greater than: {trade.LastPrice}");

            Database.UserProfiles.UpdateProfile(user);
            Database.Trades.UpdateTrade(trade);
            Database.Save();
        }

        public IEnumerable<TradeDTO> GetAllTrades()
        {
            return Adapter.Adapt<List<TradeDTO>>(Database.Trades.FindTrades());
        }

        public TradeDTO GetTradeById(int id)
        {
            if (!IsTradeExist(id))
                throw new NotFoundException();

            try
            {
                return Adapter.Adapt<TradeDTO>(Database.Trades.GetTradeById(id));
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        public TradeDTO GetTradeByLotId(int id)
        {
            var tradePoco = Database.Trades.FindTrades(t => t.LotId == id).FirstOrDefault();

            var a = Adapter.Adapt<TradeDTO>(tradePoco);
            return a;
        }

        public IEnumerable<TradeDTO> GetUserLoseTrades(string userId)
        {
            var user = Database.UserProfiles.GetProfileById(userId);

            if (user == null)
                throw new ArgumentNullException();

            var list = user.Trades.Where(x => DateTime.Now.CompareTo(x.TradeEnd) >= 0 && x.LastRateUserId != user.Id);

            return Adapter.Adapt<List<TradeDTO>>(list);
        }

        public IEnumerable<TradeDTO> GetUserWinTrades(string userId)
        {
            var user = Database.UserProfiles.GetProfileById(userId);

            if (user == null)
                throw new ArgumentNullException();

            var list = user.Trades.Where(x => DateTime.Now.CompareTo(x.TradeEnd) >= 0 && x.LastRateUserId == user.Id);

            return Adapter.Adapt<List<TradeDTO>>(list);
        }
        //maybe break this method for smaller but more informative such as GetUserWinTrades...
        public IEnumerable<TradeDTO> GetTradesForPage(string userId, int pageNum, int pageSize, string tradeState,
            out int pagesCount, out int totalItemsCount)
        {
            IQueryable<Trade> source;
            switch (tradeState)
            {
                case "won":
                    source = UserWinTrades(userId);
                    break;
                case "lose":
                    source = UserLoseTrades(userId);
                    break;
                case "all":
                    source = UserTrades(userId);
                    break;
                default:
                    source = Database.Trades.FindTrades();
                    break;
            };

            totalItemsCount = source.Count();
            if (totalItemsCount < 1)
                throw new NotFoundException();

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var tradesForPage = source.OrderBy(t => t.TradeEnd)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradeDTO>>(tradesForPage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsTradeExist(int id)
        {
            return Database.Trades.FindTrades(t => t.Id == id).Any();
        }

        private bool IsTradeForLotAlreadyStarted(int lotId)
        {
            return Database.Trades.FindTrades().Any(t => t.LotId.Equals(lotId));
        }

        #region Queries
        private IQueryable<Trade> UserWinTrades(string userId)
        {
            return Database.Trades.FindTrades().Where(t => t.LastRateUserId == userId && DateTime.Now.CompareTo(t.TradeEnd) >= 0);
        }

        private IQueryable<Trade> UserLoseTrades(string userId)
        {
            return UserTrades(userId).Where(t => t.LastRateUserId != userId && DateTime.Now.CompareTo(t.TradeEnd) >= 0);
        }

        private IQueryable<Trade> UserTrades(string userId)
        {
            IEnumerable<int> userTradesId = Database.UserProfiles.GetProfileById(userId).Trades.Select(t => t.Id);
            return Database.Trades.FindTrades().Where(t => userTradesId.Contains(t.Id));//check work maybe "contains" is a wrong method
        }
        #endregion
    }
}