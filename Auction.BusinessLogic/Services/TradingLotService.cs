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
            lotPoco.User = Database.Users.GetUserById(lot.User.Id);
            lotPoco.Category = lot.Category is null ? Database.Categories.GetCategoryById(1) : Database.Categories.GetCategoryById(lot.Category.Id);

            Database.TradingLots.AddTradingLot(lotPoco);
            Database.Save();
        }

        public void EditLot(int lotId, TradingLotDTO lot)
        {
            if (lot == null)
                throw new ArgumentNullException(nameof(lot));

            var tradingLot = Database.TradingLots.GetTradingLotById(lotId);

            if (tradingLot == null)
                throw new NotFoundException($"Trading lot with id : {lotId}");

            if (tradingLot.IsVerified)
                throw new AuctionException("You can`t change the information about the lot after the start of the bidding");
            //cant ignore null values in mapster
            //tradingLot = Adapter.Adapt<TradingLot>(lot);

            tradingLot.Name = lot.Name;
            tradingLot.Description = lot.Description;
            tradingLot.Img = lot.Img;
            tradingLot.TradeDuration = lot.TradeDuration;
            tradingLot.Price = lot.Price;

            Database.TradingLots.UpdadeTradingLot(tradingLot);
            Database.Save();
        }

        public void RemoveLotById(int lotId)
        {
            TradingLot tradingLot = Database.TradingLots.GetTradingLotById(lotId)
                ?? throw new NotFoundException($"Trading lot with id : {lotId}");

            Database.TradingLots.DeleteTradingLotById(tradingLot.Id);
            Database.Save();
        }

        //change 
        public IEnumerable<TradingLotDTO> FindLotsByCategory(int? categoryId)
        {
            var query = categoryId.HasValue ? Database.TradingLots.FindTradingLots(l => l.CategoryId == categoryId.Value)
                : Database.TradingLots.FindTradingLots().AsQueryable();

            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(query);
        }

        public TradingLotDTO GetLotById(int lotId)
        {
            var tradingLot = Database.TradingLots.GetTradingLotById(lotId)
                ?? throw new NotFoundException($"Trading lot with id : {lotId}");

            return Adapter.Adapt<TradingLotDTO>(tradingLot);
        }

        public void ChangeLotCategory(int lotId, int categoryId)
        {
            TradingLot lot = Database.TradingLots.GetTradingLotById(lotId);
            Category category = Database.Categories.GetCategoryById(categoryId);

            if (lot == null || category == null)
                throw new ArgumentNullException();

            lot.Category = category;

            Database.TradingLots.UpdadeTradingLot(lot);
            Database.Save();
        }

        public void VerifyLot(int lotId)
        {
            TradingLot lot = Database.TradingLots.GetTradingLotById(lotId);

            if (lot == null)
                throw new ArgumentNullException(nameof(lot));

            lot.IsVerified = true;

            Database.TradingLots.UpdadeTradingLot(lot);
            Database.Save();
        }

        public IEnumerable<TradingLotDTO> FindLotsByCategoryName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                throw new ArgumentException("Category name is empty", nameof(categoryName));

            var lotsInCategory = Database.TradingLots.FindTradingLots(lot => lot.Category.Name.Equals(categoryName),
                orderBy: q => q.OrderBy(l => l.TradeDuration));

            if (lotsInCategory.Any())
                throw new NotFoundException($"Lots in category {categoryName}");

            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(lotsInCategory);
        }

        public IEnumerable<TradingLotDTO> GetLotsForPage(int pageNum, int pageSize, string category,
            out int pagesCount, out int totalItemsCount)
        {
            var source = Database.TradingLots.Entities;

            if (!string.IsNullOrEmpty(category))
                source = source.Where(l => l.Category.Name.Equals(category));

            totalItemsCount = source.Count();
            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var lotsForPage = source.OrderBy(l => l.TradeDuration)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(lotsForPage);
        }
    }
}