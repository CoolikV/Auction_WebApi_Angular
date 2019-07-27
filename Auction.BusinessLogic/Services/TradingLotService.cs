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

        public void EditLot(TradingLotDTO lot)
        {
            if (lot == null)
                throw new ArgumentNullException(nameof(lot));

            var tradingLot = Database.TradingLots.GetTradingLotById(lot.Id);

            if (tradingLot == null)
                throw new ArgumentNullException(nameof(tradingLot));

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

        public void RemoveLotById(int id)
        {
            TradingLot lot = Database.TradingLots.GetTradingLotById(id);

            if (lot == null)
                throw new ArgumentNullException(nameof(lot));

            Database.TradingLots.DeleteTradingLotById(lot.Id);
            Database.Save();
        }

        public IEnumerable<TradingLotDTO> GetAllLots()
        {
            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(Database.TradingLots.FindTradingLots());
        }

        public TradingLotDTO GetLotById(int id)
        {
            return Adapter.Adapt<TradingLotDTO>(Database.TradingLots.GetTradingLotById(id));
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

        public IEnumerable<TradingLotDTO> GetLotsForCategory(int categoryId)
        {
            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(Database.TradingLots.FindTradingLots(lot => lot.CategoryId == categoryId,
                q => q.OrderByDescending(l => l.TradeDuration)));
        }
    }
}
