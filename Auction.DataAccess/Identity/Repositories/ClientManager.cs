using Auction.DataAccess.EF;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;

namespace Auction.DataAccess.Identity.Repositories
{
    public class ClientManager : IClientManager
    {
        public AuctionContext Database { get; set; }
        public ClientManager(AuctionContext db)
        {
            Database = db;
        }

        public void Create(User user)
        {
            Database.UserProfiles.Add(user);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
