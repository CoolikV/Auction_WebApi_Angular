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
                var user = new AppUser() { UserName = "user1" };
                var user1 = new AppUser() { UserName = "user2" };
                context.Users.Add(user);
                context.Users.Add(user1);
                var userProf = new UserProfile() { Id = user.Id, Name = "user1" , BirthDate = DateTime.Now , UserName = "userName1", Surname = "surname1"};
                var userProf1 = new UserProfile() { Id = user1.Id, Name = "user2" , BirthDate = DateTime.Now , UserName = "userName2", Surname = "surname2"};
                context.UserProfiles.Add(userProf);
                context.UserProfiles.Add(userProf1);
                context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
                context.Categories.Add(new Category() { Id = 2, Name = "Category 2" });
                context.SaveChanges();

                context.TradingLots.AddRange(new List<TradingLot>()
                {
                    new TradingLot(){CategoryId = 1, User = userProf, Name = "Name1", Img = "", Description = "Desc", TradeDuration = 10},
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
                });

                context.SaveChanges();
            }
        }
    }
}
