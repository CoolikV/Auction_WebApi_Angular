using Auction.BusinessLogic.DataTransfer;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradeService : IDisposable
    {
        void StartTrade(int lotId);
        void RateTradingLot(int tradeId, string userId, double price);
        TradeDTO GetTradeById(int id);
        IEnumerable<TradeDTO> GetAllTrades();
        TradeDTO GetTradeByLotId(int lotId);
        IEnumerable<TradeDTO> GetUserLoseTrades(string userId);
        IEnumerable<TradeDTO> GetUserWinTrades(string userId);
    }
}
