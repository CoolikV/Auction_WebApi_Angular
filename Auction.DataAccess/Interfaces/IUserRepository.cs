using Auction.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetById(string id);
        void Delete(string id);
    }
}
