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
    public class TradingLotService : ITradingLotService
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }

        public TradingLotService(IUnitOfWork uow, IAdapter adapter)
        {
            Database = uow;
            Adapter = adapter;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public void CreateLot(TradingLotDTO lot)
        {
            if (lot == null || lot.User == null)
                throw new ArgumentNullException(nameof(lot));

            var lotPoco = Adapter.Adapt<TradingLot>(lot);
            lotPoco.User = Database.UserProfiles.GetProfileById(lot.User.Id);
            lotPoco.Category = lot.Category is null ? Database.Categories.GetCategoryById(1) : Database.Categories.GetCategoryById(lot.Category.Id);

            Database.TradingLots.AddTradingLot(lotPoco);
            Database.Save();
        }

        public void EditLot(int lotId, TradingLotDTO lot)
        {
            if (lot == null)
                throw new ArgumentNullException(nameof(lot));

            var tradingLot = FindTradingLotById(lotId);

            //if (tradingLot.IsVerified)
            //    throw new AuctionException("You can`t change the information about the lot after the start of the bidding");
            //cant ignore null values in mapster
            //tradingLot = Adapter.Adapt<TradingLot>(lot);

            tradingLot.Name = lot.Name;
            tradingLot.Description = lot.Description;
            tradingLot.Img = lot.Img;
            tradingLot.TradeDuration = lot.TradeDuration;
            tradingLot.Price = lot.Price;

            Database.TradingLots.UpdateTradingLot(tradingLot);
            Database.Save();
        }

        public void RemoveLotById(int lotId)
        {
            TradingLot tradingLot = FindTradingLotById(lotId);

            Database.TradingLots.DeleteTradingLotById(tradingLot.Id);
            Database.Save();
        }

        public TradingLotDTO GetLotById(int lotId)
        {
            var tradingLot = FindTradingLotById(lotId);

            return Adapter.Adapt<TradingLotDTO>(tradingLot);
        }

        public void ChangeLotCategory(int lotId, int categoryId)
        {
            TradingLot lot = FindTradingLotById(lotId);
            Category category = Database.Categories.GetCategoryById(categoryId);

            if (lot == null || category == null)
                throw new ArgumentNullException();

            lot.Category = category;

            Database.TradingLots.UpdateTradingLot(lot);
            Database.Save();
        }

        public void VerifyLot(int lotId)
        {
            TradingLot lot = FindTradingLotById(lotId);

            lot.LotStatus = DataAccess.Entities.Enums.LotStatus.Verified;

            Database.TradingLots.UpdateTradingLot(lot);
            Database.Save();
        }

        //add filtering
        public IEnumerable<TradingLotDTO> GetLotsForPage(int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            var source = FilterLots(categoryId, minPrice, maxPrice, lotName);

            totalItemsCount = source.Count();
            if (totalItemsCount < 1)
                throw new NotFoundException();

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);

            var lotsForPage = source.OrderBy(l => l.TradeDuration)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(lotsForPage);
        }

        private IQueryable<TradingLot> FilterLots(int? categoryId, double? minPrice, double? maxPrice, string name)
        {
            //think how to use Expression<Func<T,bool>> predicate or use query extension methods
            IQueryable<TradingLot> source = Database.TradingLots.FindTradingLots();

            if (categoryId.HasValue)
                source = source.Where(l => l.CategoryId == categoryId);
            if (maxPrice.HasValue)
                if (minPrice < maxPrice)
                    source = source.Where(l => l.Price >= minPrice && l.Price <= maxPrice);
                else
                    source = source.Where(l => l.Price <= maxPrice);
            else
                source = source.Where(l => l.Price >= minPrice);
            if (!string.IsNullOrEmpty(name))
                source = source.Where(l => l.Name.ToLower().Contains(name));

            return source;
        }

        private TradingLot FindTradingLotById(int id)
        {
            return Database.TradingLots.GetTradingLotById(id)
                ?? throw new NotFoundException($"Lot with id: {id}");
        }
    }
}