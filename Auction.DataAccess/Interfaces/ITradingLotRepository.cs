using Auction.DataAccess.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Interfaces
{
    public interface ITradingLotRepository : IDisposable
    {
        TradingLot GetTradingLotById(int id);
        void AddTradingLot(TradingLot tradingLot);
        void UpdateTradingLot(TradingLot tradingLot);
        void DeleteTradingLotById(int id);
        void DeleteTradingLot(TradingLot tradingLot);
        IQueryable<TradingLot> FindTradingLots(Expression<Func<TradingLot, bool>> filter = null);
    }
}
