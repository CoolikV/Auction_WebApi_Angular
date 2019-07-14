using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.BusinessLogic.DataTransfer;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradingLotService
    {
        void CreateLot(TradingLotDTO lot);
        void EditLot(TradingLotDTO lot);
        void RemoveLot(int id);
        IEnumerable<TradingLotDTO> GetAllLots();
        TradingLotDTO GetLot(int id);
        void ChangeLotCategory(int lotId, int categoryId);
        void VerifyLot(int lotId);
        IEnumerable<TradingLotDTO> GetLotsForCategory(int categoryId);
        void Dispose();
    }
}
