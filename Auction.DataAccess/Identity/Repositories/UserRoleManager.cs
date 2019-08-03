using Microsoft.AspNet.Identity;
using Auction.DataAccess.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.DataAccess.Identity.Repositories
{
    public class UserRoleManager : RoleManager<AppRole>
    {
        public UserRoleManager(RoleStore<AppRole> store) : base(store)
        {
        }
    }
}
