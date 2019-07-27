using Auction.BusinessLogic.DataTransfer;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradingLotService : IDisposable
    {
        void CreateLot(TradingLotDTO lot);
        void EditLot(TradingLotDTO lot);
        void RemoveLotById(int id);
        IEnumerable<TradingLotDTO> GetAllLots();
        TradingLotDTO GetLotById(int id);
        void ChangeLotCategory(int lotId, int categoryId);
        void VerifyLot(int lotId);
        IEnumerable<TradingLotDTO> GetLotsForCategory(int categoryId);
    }
}
