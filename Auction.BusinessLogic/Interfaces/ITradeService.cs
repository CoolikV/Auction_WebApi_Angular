using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.BusinessLogic.DataTransfer;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradeService
    {
        void StartTrade(int lotId);
        void Rate(int tradeId, string userId, double price);
        TradeDTO Get(int id);
        IEnumerable<TradeDTO> GetAll();
        TradeDTO GetTradeByLot(int lotId);
        IEnumerable<TradeDTO> GetUserLoseTrades(string userId);
        IEnumerable<TradeDTO> GetUserWinTrades(string userId);
        void Dispose();
    }
}
