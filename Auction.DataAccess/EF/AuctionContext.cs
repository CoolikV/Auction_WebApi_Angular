using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Auction.DataAccess.Entities;

namespace Auction.DataAccess.EF
{
    public class AuctionContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TradingLot> TradingLots { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AuctionContext(string connectionString) : base(connectionString) { }

    }
}
