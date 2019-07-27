using Auction.DataAccess.Entities;
using Auction.DataAccess.Identity.Entities;
using Auction.DataAccess.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Auction.DataAccess.EF
{
    public class AuctionContext : IdentityDbContext<AppUser>, IDataContext // TODO: fluentapi and initial data
    {
        public DbSet<User> UserProfiles { get; }
        public DbSet<TradingLot> TradingLots { get; }
        public DbSet<Trade> Trades { get; }
        public DbSet<Category> Categories { get; }

        public AuctionContext(string connectionString) : base(connectionString) { }
    }
}
