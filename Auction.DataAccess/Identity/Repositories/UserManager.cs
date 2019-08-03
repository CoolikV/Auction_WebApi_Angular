using Microsoft.AspNet.Identity;
using Auction.DataAccess.Identity.Entities;

namespace Auction.DataAccess.Identity.Repositories
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }
    }
}
