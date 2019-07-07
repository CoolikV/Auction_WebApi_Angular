using System;
using Auction.DataAccess.Entities;

namespace Auction.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TradingLot> TradingLots { get; }
        IGenericRepository<Trade> Trades { get; }
        IGenericRepository<Category> Categories { get; }
        void Save();
    }
}
