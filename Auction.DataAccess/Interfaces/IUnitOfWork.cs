using System;
using System.Threading.Tasks;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Identity.Repositories;

namespace Auction.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager UserManager { get; }
        IClientManager ClientManager { get; }
        UserRoleManager UserRoleManager { get; }
        Task SaveAsync();
            
        IGenericRepository<TradingLot> TradingLots { get; }
        IGenericRepository<Trade> Trades { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<User> Users { get; }
        void Save();
    }
}
