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
    /// <summary>
    /// Class for working with trades
    /// </summary>
    public class TradeService : ITradeService
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }
        ITradingLotService LotService { get; set; }
        IUserManager UserManager { get; set; }
        ICategoryService CategoryService { get; set; }

        public TradeService(IUnitOfWork uow, IAdapter adapter, ITradingLotService lotService, IUserManager userManager, ICategoryService categoryService)
        {
            Database = uow;
            Adapter = adapter;
            LotService = lotService;
            UserManager = userManager;
            CategoryService = categoryService;
        }
        /// <summary>
        /// Starts new trade
        /// </summary>
        /// <param name="newTrade">Trade to start</param>
        public void StartTrade(NewTradeDTO newTrade)
        {
            if (!LotService.IsLotExists(newTrade.LotId))
                throw new NotFoundException();
            try
            {
                if (IsTradeForLotAlreadyStarted(newTrade.LotId))
                    throw new AuctionException($"Trade for this lot has already began");

                var lot = Database.TradingLots.GetTradingLotById(newTrade.LotId);

                Database.Trades.AddTrade(new Trade
                {
                    LotId = newTrade.LotId,
                    TradingLot = lot,
                    TradeStart = DateTime.Now,
                    TradeEnd = DateTime.Now.AddDays(newTrade.TradeDuration)
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
        /// <summary>
        /// Makes rate for trade
        /// </summary>
        /// <param name="rate">Rate</param>
        /// <param name="userName">User name who makes rate</param>
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

        /// <summary>
        /// Gets trade by ID
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <returns>Trade</returns>
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

        /// <summary>
        /// Gets trades for user with filtering and pagination
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Items for one page</param>
        /// <param name="tradesState">Trade state</param>
        /// <param name="startDate">Trade start</param>
        /// <param name="endDate">Trade end</param>
        /// <param name="maxBet">Max bet sum</param>
        /// <param name="lotName">Lot name</param>
        /// <param name="pagesCount">Pages to display</param>
        /// <param name="totalItemsCount">Total found trades</param>
        /// <returns>Filtered trades</returns>
        public IEnumerable<TradeDTO> GetUserTrades(string userId, int? categoryId, int pageNum, int pageSize, string tradesState, DateTime? startDate,
            DateTime? endDate, double? maxBet, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterTradesForPage(userId, categoryId, pageNum, pageSize, tradesState, startDate, endDate, maxBet, lotName, out pagesCount, out totalItemsCount);
        }

        /// <summary>
        /// Filters trade 
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Items for one page</param>
        /// <param name="tradesState">Trade state</param>
        /// <param name="startDate">Trade start</param>
        /// <param name="endDate">Trade end</param>
        /// <param name="maxBet">Max bet sum</param>
        /// <param name="lotName">Lot name</param>
        /// <param name="pagesCount">Pages to display</param>
        /// <param name="totalItemsCount">Total found trades</param>
        /// <returns>Filtered trades</returns>
        private IEnumerable<TradeDTO> FilterTradesForPage(string userId, int? categoryId, int pageNum, int pageSize,
            string tradesState, DateTime? startDate, DateTime? endDate, double? maxBet, string lotName,
            out int pagesCount, out int totalItemsCount)
        {
            IQueryable<Trade> source = Database.Trades.FindTrades(t => t.TradeEnd >= DateTime.Now);

            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(tradesState))
            {
                if (!UserManager.IsUserWithIdExist(userId))
                    throw new AuctionException("User with this id does`t exist, check user id and try again");
                source = FilterForUserByState(source, userId, tradesState);
            }
            else
            {
                source = Database.Trades.FindTrades( t => t.TradeEnd >= DateTime.Now);
            }

            if (categoryId.HasValue && CategoryService.IsCategoryExist(categoryId.Value))
                source = source.Where(t => t.TradingLot.CategoryId == categoryId);
            if (maxBet.HasValue)
                source = FilterByMaxBet(source, maxBet.Value);
            if (!string.IsNullOrWhiteSpace(lotName))
                source = FilterByLotName(source, lotName);
            if (startDate.HasValue || endDate.HasValue)
                source = FilterByDate(source, startDate.GetValueOrDefault(DateTime.MinValue), endDate.GetValueOrDefault(DateTime.MaxValue));

            try
            {
                totalItemsCount = source.Count();
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var tradesForPage = source.OrderBy(t => t.TradeEnd)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradeDTO>>(tradesForPage);
        }

        /// <summary>
        /// Gets trades for page with filtering and pagination
        /// </summary>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Items for one page</param>
        /// <param name="startDate">Trade start</param>
        /// <param name="endDate">Trade end</param>
        /// <param name="maxBet">Max bet sum</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="lotName">Lot name</param>
        /// <param name="pagesCount">Pages to display</param>
        /// <param name="totalItemsCount">Total found trades</param>
        /// <returns></returns>
        public IEnumerable<TradeDTO> GetTradesForPage(int pageNum, int pageSize, DateTime? startDate,
            DateTime? endDate, double? maxBet, int? categoryId, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterTradesForPage(string.Empty, categoryId, pageNum, pageSize, string.Empty, startDate, endDate, maxBet, lotName, out pagesCount, out totalItemsCount);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        #region Condition check methods

        /// <summary>
        /// Checks is trade exists
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <returns>True if trade exists</returns>
        private bool IsTradeExist(int id)
        {
            return Database.Trades.FindTrades(t => t.Id == id).Any();
        }

        /// <summary>
        /// Checks is trade for lot already began
        /// </summary>
        /// <param name="lotId">Lot ID</param>
        /// <returns>True if trade for specified lot already started</returns>
        private bool IsTradeForLotAlreadyStarted(int lotId)
        {
            return Database.Trades.FindTrades().Any(t => t.LotId.Equals(lotId));
        }
        /// <summary>
        /// Check is user already have max bet on trade
        /// </summary>
        /// <param name="tradeId">Trade ID</param>
        /// <param name="userId">User name</param>
        /// <returns>True if user have max bet ons specified trade</returns>
        private bool IsUserAlreadyHaveMaxBet(int tradeId, string userId)
        {
            return Database.Trades.FindTrades(t => t.Id == tradeId && t.LastRateUserId.Equals(userId)).Any();
        }
        #endregion

        #region Queries
        /// <summary>
        /// Query for user trades
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Query with user trades</returns>
        private IQueryable<Trade> UserTrades(string userId)
        {
            return Database.UserProfiles.GetProfileById(userId).Trades.AsQueryable();
        }
        /// <summary>
        /// Gets query for user won trades
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Query with user won trades</returns>
        private IQueryable<Trade> UserWinTrades(string userId)
        {
            return Database.Trades.FindTrades().Where(t => t.LastRateUserId == userId && t.TradeEnd <= DateTime.Now);
        }

        /// <summary>
        /// Gets query for user loose trades
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Query with user loose trades</returns>
        private IQueryable<Trade> UserLoseTrades(string userId)
        {
            return UserTrades(userId).Where(t => t.LastRateUserId != userId && t.TradeEnd <= DateTime.Now);
        }
        #endregion

        #region Filter methods
        /// <summary>
        /// Filter user trades by state
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="tradesState">Trade state</param>
        /// <returns>Filtered query</returns>
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

        /// <summary>
        /// Filters trade query by max bet sum
        /// </summary>
        /// <param name="source">Query to filter</param>
        /// <param name="maxBet">Max bet sum</param>
        /// <returns>Filtered query by max bet sum </returns>
        private IQueryable<Trade> FilterByMaxBet(IQueryable<Trade> source, double maxBet)
        {
            return source.Where(t => t.LastPrice <= maxBet);
        }

        /// <summary>
        /// Filters trade query by lot name
        /// </summary>
        /// <param name="source">Query to filter</param>
        /// <param name="lotName"></param>
        /// <returns>Filtered query by lot name</returns>
        private IQueryable<Trade> FilterByLotName(IQueryable<Trade> source, string lotName)
        {
            return source.Where(t => t.TradingLot.Name.ToLower().Contains(lotName.ToLower()));
        }

        /// <summary>
        /// Filters trade query by date range
        /// </summary>
        /// <param name="source">Query to filter</param>
        /// <param name="startDate">Trade start date</param>
        /// <param name="endDate">Trade end date</param>
        /// <returns>Filtered query by date range</returns>
        private IQueryable<Trade> FilterByDate(IQueryable<Trade> source, DateTime startDate, DateTime endDate)
        {
            return source.Where(t => t.TradeStart >= startDate && t.TradeEnd <= endDate);
        }
        #endregion
    }
}