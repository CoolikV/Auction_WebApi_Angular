using Microsoft.AspNet.Identity;
using Auction.DataAccess.Identity.Entities;

namespace Auction.DataAccess.Identity.Repositories
{
    public class UserManager : UserManager<AppUser>
    {
        public UserManager(IUserStore<AppUser> store) : base(store)
        {
        }
    }
}
