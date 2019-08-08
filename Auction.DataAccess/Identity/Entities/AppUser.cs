using Microsoft.AspNet.Identity.EntityFramework;
using Auction.DataAccess.Entities;

namespace Auction.DataAccess.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}
