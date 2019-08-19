using Auction.BusinessLogic.DTOs.Trade;
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
        ITradingLotService LotService { get; set; }
        IUserManager UserManager { get; set; }

        public TradeService(IUnitOfWork uow, IAdapter adapter, ITradingLotService lotService, IUserManager userManager)
        {
            Database = uow;
            Adapter = adapter;
            LotService = lotService;
            UserManager = userManager;
        }

        public void StartTrade(int lotId)
        {
            if (!LotService.IsLotExists(lotId))
                throw new NotFoundException();
            try
            {
                if (IsTradeForLotAlreadyStarted(lotId))
                    throw new AuctionException($"Trade for this lot has already began");

                var lot = Database.TradingLots.GetTradingLotById(lotId);

                Database.Trades.AddTrade(new Trade
                {
                    LotId = lotId,
                    TradingLot = lot,
                    TradeStart = DateTime.Now,
                    TradeEnd = DateTime.Now.AddDays(lot.TradeDuration)
                });
                Database.Save();
            }
            catch(AuctionException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

        }

        public void RateTradingLot(RateDTO rate, string userName)
        {
            try
            {
                if (!IsTradeExist(rate.TradeId))
                    throw new NotFoundException($"Trade with id: {rate.TradeId}");
                if (!UserManager.IsUserWithUserNameExist(userName))
                    throw new NotFoundException($"User with user name: {userName}");

                Trade trade = Database.Trades.GetTradeById(rate.TradeId);
                UserProfile user = Database.UserProfiles.GetProfileByUserName(userName);

                if (trade.TradingLot.User.Id == user.Id)
                    throw new AuctionException("This is your lot");
                if (DateTime.Now.CompareTo(trade.TradeEnd) >= 0)
                    throw new AuctionException("This trade is over");
                if (IsUserAlreadyHaveMaxBet(rate.TradeId, user.Id))
                    throw new AuctionException("You already have max bet on this lot");

                bool isNew = user.Trades.All(t => !t.Id.Equals(trade.Id));

                if (trade.LastPrice < rate.Sum)
                {
                    trade.LastPrice = rate.Sum;
                    trade.LastRateUserId = user.Id;
                    if (isNew)
                        user.Trades.Add(trade);
                }
                else
                    throw new AuctionException($"Your price should be greater than: {trade.LastPrice}");

                Database.UserProfiles.UpdateProfile(user);
                Database.Trades.UpdateTrade(trade);
            }
            catch(NotFoundException ex)
            {
                throw ex;
            }
            catch(AuctionException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

            Database.Save();
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
        //Remove if not gonna use
        public TradeDTO GetTradeByLotId(int lotId)
        {
            // maybe change Is...Exist methods to throw not found exceptions...
            if (!LotService.IsLotExists(lotId))
                throw new NotFoundException($"Lot with id : {lotId}");
            try
            {
                return Adapter.Adapt<TradeDTO>(Database.Trades.FindTrades(t => t.LotId == lotId).Single());
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        public IEnumerable<TradeDTO> GetUserTrades(string userId, int pageNum, int pageSize, string tradesState, DateTime? startDate,
            DateTime? endDate, double? maxBet, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterLotsForPage(userId, pageNum, pageSize, tradesState, startDate, endDate, maxBet, lotName, out pagesCount, out totalItemsCount);
        }

        private IEnumerable<TradeDTO> FilterLotsForPage(string userId, int pageNum, int pageSize, string tradesState, DateTime? startDate,
            DateTime? endDate, double? maxBet, string lotName, out int pagesCount, out int totalItemsCount)
        {
            IQueryable<Trade> source = Database.Trades.FindTrades();
            
            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(tradesState))
            {
                if (!UserManager.IsUserWithIdExist(userId))
                    throw new AuctionException("User with this id does`t exist, check user id and try again");
                source = FilterForUserByState(source, userId, tradesState);
            }
            if (maxBet.HasValue)
                source = FilterByMaxBet(source, maxBet.Value);
            if (!string.IsNullOrWhiteSpace(lotName))
                source = FilterByLotName(source, lotName);
            if (startDate.HasValue || endDate.HasValue)
                source = FilterByDate(source, startDate.GetValueOrDefault(DateTime.MinValue), endDate.GetValueOrDefault(DateTime.MaxValue));

            totalItemsCount = source.Count();

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var tradesForPage = source.OrderBy(t => t.TradeEnd)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradeDTO>>(tradesForPage);
        }

        public IEnumerable<TradeDTO> GetTradesForPage(int pageNum, int pageSize, DateTime? startDate,
            DateTime? endDate, double? maxBet, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterLotsForPage(string.Empty, pageNum, pageSize, string.Empty, startDate, endDate, maxBet, lotName, out pagesCount, out totalItemsCount);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        #region Condition check methods
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

        private bool IsUserAlreadyHaveMaxBet(int tradeId, string userId)
        {
            return Database.Trades.FindTrades(t => t.Id == tradeId && t.LastRateUserId.Equals(userId)).Any();
        }
        #endregion

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
            return Database.UserProfiles.GetProfileById(userId).Trades.AsQueryable();
        }
        #endregion

        #region Filter methods
        private IQueryable<Trade> FilterForUserByState(IQueryable<Trade> source, string userId, string tradesState)
        {
            switch (tradesState)
            {
                case "won":
                    source = UserWinTrades(userId);
                    break;
                case "lose":
                    source = UserLoseTrades(userId);
                    break;
                default:
                    source = UserTrades(userId);
                    break;
            };

            return source;
        }

        private IQueryable<Trade> FilterByMaxBet(IQueryable<Trade> source, double maxBet)
        {
            return source.Where(t => t.LastPrice <= maxBet);
        }

        private IQueryable<Trade> FilterByLotName(IQueryable<Trade> source, string lotName)
        {
            return source.Where(t => t.TradingLot.Name.ToLower().Contains(lotName.ToLower()));
        }

        private IQueryable<Trade> FilterByDate(IQueryable<Trade> source, DateTime startDate, DateTime endDate)
        {
            return source.Where(t => t.TradeStart >= startDate && t.TradeEnd <= endDate);
        }
        #endregion
    }
}