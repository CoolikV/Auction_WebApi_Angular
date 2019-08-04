using Auction.DataAccess.Entities;
using Auction.DataAccess.Identity.Entities;
using Auction.DataAccess.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Auction.DataAccess.EF
{
    public class AuctionContext : IdentityDbContext<AppUser>, IDataContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<TradingLot> TradingLots { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AuctionContext(string connectionString) : base(connectionString) { }

        static AuctionContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public class DbInitializer : DropCreateDatabaseAlways<AuctionContext>
        {
            protected override void Seed(AuctionContext context)
            {
                context.Roles.Add(new IdentityRole("user"));
                var user = new AppUser() { UserName = "coolik" };
                context.Users.Add(user);
                var userProf = new UserProfile() { Id = user.Id, Name = "coolik1" , BirthDate = DateTime.Now , UserName = "coolik1", Surname = "surname"};
                context.UserProfiles.Add(userProf);
                context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
                context.Categories.Add(new Category() { Id = 2, Name = "Category 2" });
                context.SaveChanges();

                context.TradingLots.AddRange(new List<TradingLot>()
                {
                    new TradingLot(){CategoryId = 1, User = userProf,IsVerified = true, Name = "Name1", Img = new byte[13], Description = "Desc", TradeDuration = 10},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 1, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                    new TradingLot(){CategoryId = 2, User = userProf},
                });

                context.SaveChanges();
            }
        }
    }
}
