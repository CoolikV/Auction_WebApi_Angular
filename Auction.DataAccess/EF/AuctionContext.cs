using Auction.DataAccess.Entities;
using Auction.DataAccess.Entities.Enums;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Trading lot constraints
            modelBuilder.Entity<TradingLot>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<TradingLot>()
                .Property(t => t.Description)
                .IsOptional()
                .HasMaxLength(150);

            modelBuilder.Entity<TradingLot>()
                .Property(t => t.Img)
                .IsRequired();

            modelBuilder.Entity<TradingLot>()
                .Property(t => t.Price)
                .IsRequired();

            modelBuilder.Entity<TradingLot>()
                .Property(t => t.TradeDuration)
                .IsRequired();

            modelBuilder.Entity<TradingLot>()
                .Property(t => t.LotStatus)
                .IsRequired();

            modelBuilder.Entity<TradingLot>()
                .HasRequired(t => t.Category);

            modelBuilder.Entity<TradingLot>()
                .HasRequired(t => t.User);
            #endregion

            #region Category constraints
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            #endregion

            #region Trade constraints
            modelBuilder.Entity<Trade>()
                .Property(t => t.TradeStart)
                .IsRequired();

            modelBuilder.Entity<Trade>()
                .Property(t => t.TradeEnd)
                .IsRequired();

            //NOT SHURE IS THAT RIGHT
            //modelBuilder.Entity<Trade>()
            //    .HasOptional(t => t.LastRated)
            //    .WithOptionalDependent();
            //NOT SHURE IS THAT RIGHT

            modelBuilder.Entity<Trade>()
                .HasRequired(t => t.TradingLot);
            #endregion

            #region User profile constraints
            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Surname)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.UserName)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.BirthDate)
                .IsRequired();

            modelBuilder.Entity<UserProfile>()
                .HasRequired(u => u.AppUser);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        static AuctionContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public class DbInitializer : DropCreateDatabaseAlways<AuctionContext>
        {
            protected override void Seed(AuctionContext context)
            {
                context.Roles.Add(new IdentityRole("user"));
                context.Roles.Add(new IdentityRole("admin"));
                context.Roles.Add(new IdentityRole("manager"));

                //think how to seed appusers and their profiles to use it when creating a lots
                var user = new AppUser() { UserName = "user1",  };
                var user1 = new AppUser() { UserName = "user2" };
                context.Users.Add(user);
                context.Users.Add(user1);

                var userProfiles = new List<UserProfile>()
                {
                    new UserProfile(){Id = user.Id, Name = "user1" , BirthDate = DateTime.Now , UserName = "userName1", Surname = "surname1"},
                    new UserProfile() { Id = user1.Id, Name = "user2" , BirthDate = DateTime.Now , UserName = "userName2", Surname = "surname2"}
                };

                context.UserProfiles.AddRange(userProfiles);
                context.SaveChanges();

                List<Category> categories = new List<Category>()
                {
                    new Category(){Name = "Miscellaneous"},
                    new Category(){Name = "Antiques"},
                    new Category(){Name = "Art"},
                    new Category(){Name = "Cars, Parts & Vechicles"},
                    new Category(){Name = "Cell Phones"},
                    new Category(){Name = "Clothing"},
                    new Category(){Name = "Coins"},
                    new Category(){Name = "Collectibles"},
                    new Category(){Name = "Computers & Networking"},
                    new Category(){Name = "Coins"},
                    new Category(){Name = "DVDs & Movies"},
                    new Category(){Name = "Cameras & Photo"},
                    new Category(){Name = "Shoes & Accessories"},
                    new Category(){Name = "Jewerly & Watches"},
                    new Category(){Name = "Books"}
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();

                var tradingLots = new List<TradingLot>()
                {
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage1", Description = "Some shitty thing1", Img = "/", Price = 11.2, TradeDuration = 4,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage2", Description = "Some shitty thing1", Img = "/", Price = 12.2, TradeDuration = 4,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},

                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},

                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]},
                    new TradingLot(){ Name = "Garbage", Description = "Some shitty thing", Img = "/", Price = 10.2, TradeDuration = 3,
                        LotStatus = LotStatus.NotVerified, CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[0].Id, User = userProfiles[0]}
                };
               
                context.SaveChanges();
            }
        }
    }
}
