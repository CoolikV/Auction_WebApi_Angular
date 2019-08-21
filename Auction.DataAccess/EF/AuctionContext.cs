using Auction.DataAccess.Entities;
using Auction.DataAccess.EntityConfigurations;
using Auction.DataAccess.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Auction.DataAccess.EF
{
    public class AuctionContext : IdentityDbContext<AppUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<TradingLot> TradingLots { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AuctionContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TradingLotConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new TradeConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
