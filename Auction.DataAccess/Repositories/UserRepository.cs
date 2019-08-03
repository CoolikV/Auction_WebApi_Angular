using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        internal readonly IDataContext Database;
        internal readonly DbSet<UserProfile> dbSet;

        public UserRepository(IDataContext context)
        {
            Database = context;
            dbSet = context.Set<UserProfile>();
        }

        public IQueryable<UserProfile> Entities => dbSet;

        public void AddUser(UserProfile user)
        {
            dbSet.Add(user);
        }

        public void DeleteUser(UserProfile user)
        {
            if (Database.Entry(user).State == EntityState.Detached)
                dbSet.Attach(user);

            dbSet.Remove(user);
        }

        public void DeleteUserById(string id)
        {
            UserProfile user = dbSet.Find(id);
            DeleteUser(user);
        }

        public IEnumerable<UserProfile> FindUsers(Expression<Func<UserProfile, bool>> filter = null,
            Func<IQueryable<UserProfile>, IOrderedQueryable<UserProfile>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<UserProfile> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public UserProfile GetUserById(string id)
        {
            return dbSet.Find(id);
        }

        public void UpdadeUser(UserProfile user)
        {
            dbSet.Attach(user);
            Database.Entry(user).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
