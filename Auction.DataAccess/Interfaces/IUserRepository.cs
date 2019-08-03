using Auction.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Auction.DataAccess.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        UserProfile GetUserById(string id);
        void AddUser(UserProfile user);
        void UpdadeUser(UserProfile user);
        void DeleteUser(UserProfile user);
        void DeleteUserById(string id);
        IEnumerable<UserProfile> FindUsers(Expression<Func<UserProfile, bool>> filter = null,
            Func<IQueryable<UserProfile>, IOrderedQueryable<UserProfile>> orderBy = null,
            string includeProperties = "");
        IQueryable<UserProfile> Entities { get; }
    }
}
