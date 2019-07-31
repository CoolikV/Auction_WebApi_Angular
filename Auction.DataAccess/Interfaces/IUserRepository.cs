using Auction.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Auction.DataAccess.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        User GetUserById(string id);
        void AddUser(User user);
        void UpdadeUser(User user);
        void DeleteUser(User user);
        void DeleteUserById(string id);
        IEnumerable<User> FindUsers(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            string includeProperties = "");
        IQueryable<User> Users { get; }
    }
}
