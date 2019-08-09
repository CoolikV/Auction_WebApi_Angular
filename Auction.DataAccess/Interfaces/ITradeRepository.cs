using Auction.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Interfaces
{
    public interface ITradeRepository : IDisposable
    {
        Trade GetTradeById(int id);
        void AddTrade(Trade trade);
        void UpdateTrade(Trade trade);
        void DeleteTradeById(int id);
        void DeleteTrade(Trade trade);
        IEnumerable<Trade> FindTrades(Expression<Func<Trade, bool>> filter = null,
            Func<IQueryable<Trade>, IOrderedQueryable<Trade>> orderBy = null);
        IQueryable<Trade> Entities { get; }
    }
}
