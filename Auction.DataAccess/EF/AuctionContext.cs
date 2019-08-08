using Auction.DataAccess.Entities;
using Auction.DataAccess.Entities.Enums;
using Auction.DataAccess.EntityConfigurations;
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
            modelBuilder.Configurations.Add(new TradingLotConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new TradeConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());

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
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole("user"),
                    new IdentityRole("admin"),
                    new IdentityRole("manager")
                };
                context.Roles.Add(roles[0]);
                context.Roles.Add(roles[1]);
                context.Roles.Add(roles[2]);
                context.SaveChanges();

                var userProfiles = new List<UserProfile>()
                {
                    new UserProfile(){ Name = "Admin", Surname = "Adminovich", BirthDate = new DateTime(1999,2,27) },
                    new UserProfile(){ Name = "Manager", Surname = "Managerovich", BirthDate = new DateTime(1991,9,20) },
                    new UserProfile(){ Name = "Вася", Surname = "Пупкин", BirthDate = new DateTime(1976,1,10) },
                    new UserProfile(){ Name = "Андрей", Surname = "Баранов", BirthDate = new DateTime(1988,11,24) },
                    new UserProfile(){ Name = "Антон", Surname = "Якушев", BirthDate = new DateTime(1994,6,13) },
                    new UserProfile(){ Name = "Владимир", Surname = "Гусев", BirthDate = new DateTime(1965,3,12) },
                    new UserProfile(){ Name = "Анатолий", Surname = "Коновалов", BirthDate = new DateTime(1987,4,23) },
                    new UserProfile(){ Name = "Александр", Surname = "Туров", BirthDate = new DateTime(1983,9,26) },
                    new UserProfile(){ Name = "Виктор", Surname = "Сафонов", BirthDate = new DateTime(1998,12,29) },
                    new UserProfile(){ Name = "Иван", Surname = "Филиппов", BirthDate = new DateTime(1974,11,1) },
                    new UserProfile(){ Name = "Александр", Surname = "Самсонов", BirthDate = new DateTime(1966,5,4) },
                    new UserProfile(){ Name = "Даниил", Surname = "Родионов", BirthDate = new DateTime(1989,10,20) },
                    new UserProfile(){ Name = "Юрий", Surname = "Евсеев", BirthDate = new DateTime(1999,10,21) },
                };

                var hasher = new Microsoft.AspNet.Identity.PasswordHasher();
                var userAccounts = new List<AppUser>()
                {
                    new AppUser(){ UserName = "admin", Email = "admin@gmail.com", UserProfile = userProfiles[0], PasswordHash = hasher.HashPassword("admin1999ad") },
                    new AppUser(){ UserName = "manager", Email = "manager@gmail.com", UserProfile = userProfiles[1], PasswordHash = hasher.HashPassword("manager1999man") },
                    new AppUser(){ UserName = "vasya", Email = "v_pupkin@gmail.com", UserProfile = userProfiles[2], PasswordHash = hasher.HashPassword("pupkin12345") },
                    new AppUser(){ UserName = "andrey", Email = "a_baranov@gmail.com", UserProfile = userProfiles[3], PasswordHash = hasher.HashPassword("baranov12345") },
                    new AppUser(){ UserName = "anton", Email = "a_yakushev@gmail.com", UserProfile = userProfiles[4], PasswordHash = hasher.HashPassword("yakushev12345") },
                    new AppUser(){ UserName = "vladimir", Email = "v_gusev@gmail.com", UserProfile = userProfiles[5], PasswordHash = hasher.HashPassword("gusev12345") },
                    new AppUser(){ UserName = "anatoliy", Email = "a_konovalov@gmail.com", UserProfile = userProfiles[6], PasswordHash = hasher.HashPassword("konovalov12345") },
                    new AppUser(){ UserName = "alex", Email = "a_turov@gmail.com", UserProfile = userProfiles[7], PasswordHash = hasher.HashPassword("turov12345") },
                    new AppUser(){ UserName = "viktor", Email = "v_safonov@gmail.com", UserProfile = userProfiles[8], PasswordHash = hasher.HashPassword("safonov12345") },
                    new AppUser(){ UserName = "ivan", Email = "i_filipov@gmail.com", UserProfile = userProfiles[9], PasswordHash = hasher.HashPassword("filipov12345") },
                    new AppUser(){ UserName = "alexandr", Email = "a_samsonov@gmail.com", UserProfile = userProfiles[10], PasswordHash = hasher.HashPassword("samsonov12345") },
                    new AppUser(){ UserName = "daniyl", Email = "d_rodionov@gmail.com", UserProfile = userProfiles[11], PasswordHash = hasher.HashPassword("rodionov12345") },
                    new AppUser(){ UserName = "yuriy", Email = "y_eseev@gmail.com", UserProfile = userProfiles[12], PasswordHash = hasher.HashPassword("essev12345") }
                };

                int ind = 0;
                foreach (var profile in userProfiles)
                {
                    userAccounts[ind].SecurityStamp = Guid.NewGuid().ToString();
                    context.Users.Add(userAccounts[ind]);

                    profile.AppUser = userAccounts[ind];
                    profile.UserName = userAccounts[ind].UserName;
                    profile.Id = userAccounts[ind].Id;

                    ind++;
                }
                context.UserProfiles.AddRange(userProfiles);
                context.SaveChanges();

                ind = 0;
                string[] commands = new string[userAccounts.Count];
                foreach(var user in userAccounts)
                {
                    if (ind == 0)
                        context.Database.ExecuteSqlCommand($@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId] ,[RoleId]) VALUES ('{userAccounts[ind].Id}', '{roles[1].Id}')");
                    else if (ind == 1)
                        context.Database.ExecuteSqlCommand($@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId] ,[RoleId]) VALUES ('{userAccounts[ind].Id}', '{roles[2].Id}')");
                    else
                        context.Database.ExecuteSqlCommand($@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId] ,[RoleId]) VALUES ('{userAccounts[ind].Id}', '{roles[0].Id}')");
                    ind++;
                }
                    
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

                context.TradingLots.AddRange(tradingLots);
                context.SaveChanges();
            }
        }
    }
}
