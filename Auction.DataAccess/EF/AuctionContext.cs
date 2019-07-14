using System.Data.Entity;
using Auction.DataAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Auction.DataAccess.Identity.Entities;

namespace Auction.DataAccess.EF
{
    public class AuctionContext : IdentityDbContext<AppUser>
    {
        public DbSet<User> UserProfiles { get; set; }
        public DbSet<TradingLot> TradingLots { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AuctionContext(string connectionString) : base(connectionString) { }

    }
}
