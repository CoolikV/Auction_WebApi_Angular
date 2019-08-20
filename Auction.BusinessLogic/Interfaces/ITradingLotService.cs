using Auction.BusinessLogic.DTOs.TradingLot;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradingLotService : IDisposable
    {
        void CreateLot(NewTradingLotDTO lot, string userName, string folder);
        void EditLot(int lotId, NewTradingLotDTO lot, bool isManager);
        void RemoveLotById(int id);

        IEnumerable<TradingLotDTO> GetLotsForPage(int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount);
        IEnumerable<TradingLotDTO> GetLotsForUser(string userId, int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount);
        TradingLotDTO GetLotById(int id);

        void ChangeLotCategory(int lotId, int categoryId);

        bool IsLotExists(int id);
    }
}