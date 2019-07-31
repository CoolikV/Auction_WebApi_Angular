using Auction.DataAccess.Entities;
using Auction.DataAccess.Identity.Entities;
using Auction.DataAccess.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace Auction.DataAccess.EF
{
    public class AuctionContext : IdentityDbContext<AppUser>, IDataContext
    {
        public DbSet<User> UserProfiles { get; set; }
        public DbSet<TradingLot> TradingLots { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AuctionContext(string connectionString) : base(connectionString) { }

        static AuctionContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public class DbInitializer : DropCreateDatabaseIfModelChanges<AuctionContext>
        {
            protected override void Seed(AuctionContext context)
            {
                context.Roles.Add(new IdentityRole("user"));

                context.Categories.Add(new Category() { Id = 1, Name = "Test1" });
                context.Categories.Add(new Category() { Id = 2, Name = "Test2" });
                context.SaveChanges();

                context.TradingLots.AddRange(new List<TradingLot>()
                {
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 1},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2},
                    new TradingLot(){CategoryId = 2}
                });

                context.SaveChanges();
            }
        }
    }
}
