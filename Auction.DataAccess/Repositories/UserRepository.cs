using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using Auction.DataAccess.Interfaces;
using Auction.DataAccess.EF;
using Auction.DataAccess.Entities;

namespace Auction.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AuctionContext context) 
            : base(context) { }

        public void Delete(string id)
        {
            User user = dbSet.Find(id);
            base.Delete(user);
        }

        public User GetById(string id)
        {
            return dbSet.Find(id);
        }  
    }
}
