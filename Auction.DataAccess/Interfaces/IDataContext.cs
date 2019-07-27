using Auction.DataAccess.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Auction.DataAccess.Interfaces
{
    public interface IDataContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<User> UserProfiles { get; }
        DbSet<TradingLot> TradingLots { get; }
        DbSet<Trade> Trades { get; }
        DbSet<Category> Categories { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
