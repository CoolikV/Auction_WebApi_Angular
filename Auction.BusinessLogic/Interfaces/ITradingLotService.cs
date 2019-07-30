using Auction.BusinessLogic.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradingLotService : IDisposable
    {
        void CreateLot(TradingLotDTO lot);
        void EditLot(int lotId, TradingLotDTO lot);
        void RemoveLotById(int id);
        IQueryable<TradingLotDTO> FindLots(int? categoryId);
        TradingLotDTO GetLotById(int id);
        void ChangeLotCategory(int lotId, int categoryId);
        void VerifyLot(int lotId);
    }
}
