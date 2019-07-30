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
        //test using mapper
        public void CreateLot(TradingLotDTO lot)
        {
            if (lot == null || lot.User == null)
                throw new ArgumentNullException(nameof(lot));
            //try to automapper for most of properties and set user and category after that
            var newLot = new TradingLot()
            {
                Name = lot.Name,
                Price = lot.Price,
                Description = lot.Description,
                Img = lot.Img,
                TradeDuration = lot.TradeDuration,
                User = Database.Users.GetUserById(lot.User.Id),
                Category = lot.Category == null ? Database.Categories.GetCategoryById(1) : Database.Categories.GetCategoryById(lot.Category.Id)
            };

            Database.TradingLots.AddTradingLot(newLot);
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
            //maybe mapping?
            tradingLot.Name = lot.Name;
            tradingLot.Description = lot.Description;
            tradingLot.Img = lot.Img;
            tradingLot.TradeDuration = lot.TradeDuration;

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

        public IQueryable<TradingLotDTO> FindLots(int? categoryId)
        {
            var query = categoryId.HasValue ? Database.TradingLots.FindTradingLots(l => l.CategoryId == categoryId.Value)
                : Database.TradingLots.FindTradingLots();

            return Adapter.Adapt<IQueryable<TradingLotDTO>>(query);
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
    }
}