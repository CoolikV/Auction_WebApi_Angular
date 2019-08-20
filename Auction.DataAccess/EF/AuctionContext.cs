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

        public AuctionContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TradingLotConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new TradeConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public class DbInitializer : DropCreateDatabaseAlways<AuctionContext>
        {
            protected override void Seed(AuctionContext context)
            {
                var roles = new List<IdentityRole>()
                {
                    new AppRole(){ Name = "user"},
                    new AppRole(){ Name = "admin"},
                    new AppRole(){ Name = "manager"},
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
                    new TradingLot(){ Name = "Lot1", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                         CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[2].Id, User = userProfiles[2]},
                    new TradingLot(){ Name = "Lot2", Description = "Some shitty thing", Img = "df.jpg", Price = 11.2, TradeDuration = 4,
                         CategoryId = categories[0].Id, Category = categories[1], UserId = userProfiles[2].Id, User = userProfiles[2]},
                    new TradingLot(){ Name = "Lot3", Description = "Some shitty thing", Img = "df.jpg", Price = 12.2, TradeDuration = 4,
                         CategoryId = categories[0].Id, Category = categories[2], UserId = userProfiles[2].Id, User = userProfiles[2]},
                    new TradingLot(){ Name = "Lot4", Description = "Some shitty thing", Img = "df.jpg", Price = 400, TradeDuration = 3,
                         CategoryId = categories[1].Id, Category = categories[3], UserId = userProfiles[2].Id, User = userProfiles[2]},

                    new TradingLot(){ Name = "Lot5", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                         CategoryId = categories[1].Id, Category = categories[4], UserId = userProfiles[3].Id, User = userProfiles[3]},
                    new TradingLot(){ Name = "Lot6", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                         CategoryId = categories[1].Id, Category = categories[5], UserId = userProfiles[3].Id, User = userProfiles[3]},
                    new TradingLot(){ Name = "Lot7", Description = "Some shitty thing", Img = "df.jpg", Price = 150, TradeDuration = 3,
                         CategoryId = categories[1].Id, Category = categories[6], UserId = userProfiles[3].Id, User = userProfiles[3]},
                    new TradingLot(){ Name = "Lot8", Description = "Some shitty thing", Img = "df.jpg", Price = 200, TradeDuration = 3,
                         CategoryId = categories[1].Id, Category = categories[7], UserId = userProfiles[3].Id, User = userProfiles[3]},

                    new TradingLot(){ Name = "Lot9", Description = "Some shitty thing", Img = "df.jpg", Price = 250, TradeDuration = 3,
                         CategoryId = categories[1].Id, Category = categories[8], UserId = userProfiles[4].Id, User = userProfiles[4]},
                    new TradingLot(){ Name = "Lot10", Description = "Some shitty thing", Img = "df.jpg", Price = 400, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[9], UserId = userProfiles[4].Id, User = userProfiles[4]},
                    new TradingLot(){ Name = "Lot11", Description = "Some shitty thing", Img = "df.jpg", Price = 400, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[10], UserId = userProfiles[4].Id, User = userProfiles[4]},
                    new TradingLot(){ Name = "Lot12", Description = "Some shitty thing", Img = "df.jpg", Price = 500, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[10], UserId = userProfiles[4].Id, User = userProfiles[4]},

                    new TradingLot(){ Name = "Lot13", Description = "Some shitty thing", Img = "df.jpg", Price = 500, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[10], UserId = userProfiles[5].Id, User = userProfiles[5]},
                    new TradingLot(){ Name = "Lot14", Description = "Some shitty thing", Img = "df.jpg", Price = 600, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[10], UserId = userProfiles[5].Id, User = userProfiles[5]},
                    new TradingLot(){ Name = "Lot15", Description = "Some shitty thing", Img = "df.jpg", Price = 1100, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[10], UserId = userProfiles[5].Id, User = userProfiles[5]},

                    new TradingLot(){ Name = "Lot16", Description = "Some shitty thing", Img = "df.jpg", Price = 1100, TradeDuration = 3,
                         CategoryId = categories[2].Id, Category = categories[1], UserId = userProfiles[6].Id, User = userProfiles[6]},
                    new TradingLot(){ Name = "Lot17", Description = "Some shitty thing", Img = "df.jpg", Price = 1100, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[1], UserId = userProfiles[6].Id, User = userProfiles[6]},
                    new TradingLot(){ Name = "Lot18", Description = "Some shitty thing", Img = "df.jpg", Price = 300, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[1], UserId = userProfiles[6].Id, User = userProfiles[6]},

                    new TradingLot(){ Name = "Lot19", Description = "Some shitty thing", Img = "df.jpg", Price = 400, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[1], UserId = userProfiles[7].Id, User = userProfiles[7]},
                    new TradingLot(){ Name = "Lot20", Description = "Some shitty thing", Img = "df.jpg", Price = 500, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[2], UserId = userProfiles[7].Id, User = userProfiles[7]},

                    new TradingLot(){ Name = "Lot21", Description = "Some shitty thing", Img = "df.jpg", Price = 600, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[3], UserId = userProfiles[8].Id, User = userProfiles[8]},
                    new TradingLot(){ Name = "Lot22", Description = "Some shitty thing", Img = "df.jpg", Price = 700, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[3], UserId = userProfiles[8].Id, User = userProfiles[8]},

                    new TradingLot(){ Name = "Lot23", Description = "Some shitty thing", Img = "df.jpg", Price = 800, TradeDuration = 3,
                         CategoryId = categories[3].Id, Category = categories[3], UserId = userProfiles[9].Id, User = userProfiles[9]}
                    //new TradingLot(){ Name = "Lot24", Description = "Some shitty thing", Img = "df.jpg", Price = 900, TradeDuration = 3,
                    //     CategoryId = categories[4].Id, Category = categories[4], UserId = userProfiles[9].Id, User = userProfiles[9]},

                    //new TradingLot(){ Name = "Lot25", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[5].Id, Category = categories[5], UserId = userProfiles[10].Id, User = userProfiles[10]},
                    //new TradingLot(){ Name = "Lot26", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[5].Id, Category = categories[5], UserId = userProfiles[11].Id, User = userProfiles[11]},
                    //new TradingLot(){ Name = "Lot27", Description = "Some shitty thing", Img = "df.jpg", Price = 120, TradeDuration = 3,
                    //     CategoryId = categories[5].Id, Category = categories[5], UserId = userProfiles[11].Id, User = userProfiles[11]},

                    //new TradingLot(){ Name = "Lot28", Description = "Some shitty thing", Img = "df.jpg", Price = 150, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[12].Id, User = userProfiles[12]},
                    //new TradingLot(){ Name = "Lot29", Description = "Some shitty thing", Img = "df.jpg", Price = 300, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[12].Id, User = userProfiles[12]},

                    //new TradingLot(){ Name = "Lot30", Description = "Some shitty thing", Img = "df.jpg", Price = 200, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[3].Id, User = userProfiles[3]},
                    //new TradingLot(){ Name = "Lot31", Description = "Some shitty thing", Img = "df.jpg", Price = 300, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[3].Id, User = userProfiles[3]},
                    //new TradingLot(){ Name = "Lot32", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[3].Id, User = userProfiles[3]},
                    //new TradingLot(){ Name = "Lot33", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[3].Id, User = userProfiles[3]},

                    //new TradingLot(){ Name = "Lot34", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[4].Id, User = userProfiles[4]},

                    //new TradingLot(){ Name = "Lot35", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[5].Id, User = userProfiles[5]},

                    //new TradingLot(){ Name = "Lot36", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[6].Id, User = userProfiles[6]},
                    //new TradingLot(){ Name = "Lot37", Description = "Some shitty thing", Img = "df.jpg", Price = 100, TradeDuration = 3,
                    //     CategoryId = categories[0].Id, Category = categories[0], UserId = userProfiles[6].Id, User = userProfiles[6]}
                };

                context.TradingLots.AddRange(tradingLots);
                context.SaveChanges();

                var trades = new List<Trade>()
                {
                    new Trade(){ LotId = 1, LastPrice = 400, TradeStart = DateTime.Now, TradingLot = tradingLots[0],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[0].TradeDuration), LastRateUserId = userAccounts[3].Id},

                    new Trade(){ LotId = 2, LastPrice = 100, TradeStart = DateTime.Now, TradingLot = tradingLots[1],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[1].TradeDuration), LastRateUserId = userAccounts[3].Id},

                    new Trade(){ LotId = 3, LastPrice = 100, TradeStart = DateTime.Now, TradingLot = tradingLots[2],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[2].TradeDuration), LastRateUserId = userAccounts[3].Id},

                    new Trade(){ LotId = 4, LastPrice = 500, TradeStart = DateTime.Now, TradingLot = tradingLots[3],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[3].TradeDuration), LastRateUserId = userAccounts[4].Id},

                    new Trade(){ LotId = 5, LastPrice = 200, TradeStart = DateTime.Now, TradingLot = tradingLots[4],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[4].TradeDuration), LastRateUserId = userAccounts[4].Id},

                    new Trade(){ LotId = 6, LastPrice = 300, TradeStart = DateTime.Now, TradingLot = tradingLots[5],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[5].TradeDuration), LastRateUserId = userAccounts[4].Id},

                    new Trade(){ LotId = 7, LastPrice = 300, TradeStart = DateTime.Now, TradingLot = tradingLots[6],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[6].TradeDuration), LastRateUserId = userAccounts[5].Id},

                    new Trade(){ LotId = 8, LastPrice = 350, TradeStart = DateTime.Now, TradingLot = tradingLots[7],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[7].TradeDuration), LastRateUserId = userAccounts[6].Id},

                    new Trade(){ LotId = 9, LastPrice = 300, TradeStart = DateTime.Now, TradingLot = tradingLots[8],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[8].TradeDuration), LastRateUserId = userAccounts[6].Id},

                    new Trade(){ LotId = 10, LastPrice = 500, TradeStart = DateTime.Now, TradingLot = tradingLots[9],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[9].TradeDuration), LastRateUserId = userAccounts[7].Id},

                    new Trade(){ LotId = 11, LastPrice = 500, TradeStart = DateTime.Now, TradingLot = tradingLots[10],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[10].TradeDuration), LastRateUserId = userAccounts[7].Id},

                    new Trade(){ LotId = 12, LastPrice = 600, TradeStart = DateTime.Now, TradingLot = tradingLots[11],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[11].TradeDuration), LastRateUserId = userAccounts[7].Id},

                    new Trade(){ LotId = 13, LastPrice = 600, TradeStart = DateTime.Now, TradingLot = tradingLots[12],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[12].TradeDuration), LastRateUserId = userAccounts[8].Id},

                    new Trade(){ LotId = 14, LastPrice = 700, TradeStart = DateTime.Now, TradingLot = tradingLots[13],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[13].TradeDuration), LastRateUserId = userAccounts[8].Id},

                    new Trade(){ LotId = 15, LastPrice = 1300, TradeStart = DateTime.Now, TradingLot = tradingLots[14],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[14].TradeDuration), LastRateUserId = userAccounts[9].Id},

                    new Trade(){ LotId = 16, LastPrice = 1300, TradeStart = DateTime.Now, TradingLot = tradingLots[15],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[15].TradeDuration), LastRateUserId = userAccounts[9].Id},

                    new Trade(){ LotId = 17, LastPrice = 1500, TradeStart = DateTime.Now, TradingLot = tradingLots[16],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[16].TradeDuration), LastRateUserId = userAccounts[10].Id},

                    new Trade(){ LotId = 18, LastPrice = 400, TradeStart = DateTime.Now, TradingLot = tradingLots[17],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[17].TradeDuration), LastRateUserId = userAccounts[10].Id},

                    new Trade(){ LotId = 19, LastPrice = 500, TradeStart = DateTime.Now, TradingLot = tradingLots[18],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[18].TradeDuration), LastRateUserId = userAccounts[3].Id},

                    new Trade(){ LotId = 20, LastPrice = 600, TradeStart = DateTime.Now, TradingLot = tradingLots[19],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[19].TradeDuration), LastRateUserId = userAccounts[4].Id},

                    new Trade(){ LotId = 21, LastPrice = 700, TradeStart = DateTime.Now, TradingLot = tradingLots[20],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[20].TradeDuration), LastRateUserId = userAccounts[4].Id},

                    new Trade(){ LotId = 22, LastPrice = 800, TradeStart = DateTime.Now, TradingLot = tradingLots[21],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[21].TradeDuration), LastRateUserId = userAccounts[4].Id},

                    new Trade(){ LotId = 23, LastPrice = 900, TradeStart = DateTime.Now, TradingLot = tradingLots[22],
                        TradeEnd = DateTime.Now.AddDays(tradingLots[22].TradeDuration), LastRateUserId = userAccounts[5].Id}
                };

                context.Trades.AddRange(trades);
                context.SaveChanges();
            }
        }
    }
}
