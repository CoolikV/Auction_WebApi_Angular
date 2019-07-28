using Auction.DataAccess.Identity.Repositories;
using System;
using System.Threading.Tasks;

namespace Auction.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        UserRoleManager UserRoleManager { get; }
        Task SaveAsync();
        
        ITradingLotRepository TradingLots { get; }
        ITradeRepository Trades { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
