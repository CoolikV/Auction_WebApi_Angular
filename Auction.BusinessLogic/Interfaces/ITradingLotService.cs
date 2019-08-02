using Auction.BusinessLogic.DataTransfer;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradingLotService : IDisposable
    {
        void CreateLot(TradingLotDTO lot);
        void EditLot(int lotId, TradingLotDTO lot);
        void RemoveLotById(int id);
        IEnumerable<TradingLotDTO> GetLotsForPage(int pageNum, int count, string category, out int pagesCount, out int totalItemsCount);
        TradingLotDTO GetLotById(int id);
        IEnumerable<TradingLotDTO> FindLotsByCategoryName(string categoryName);
        void ChangeLotCategory(int lotId, int categoryId);
        void VerifyLot(int lotId);
    }
}