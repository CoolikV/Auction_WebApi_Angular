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
        internal readonly DbSet<User> dbSet;

        public UserRepository(IDataContext context)
        {
            Database = context;
            dbSet = context.Set<User>();
        }

        public void AddUser(User user)
        {
            dbSet.Add(user);
        }

        public void DeleteUser(User user)
        {
            if (Database.Entry(user).State == EntityState.Detached)
                dbSet.Attach(user);

            dbSet.Remove(user);
        }

        public void DeleteUserById(string id)
        {
            User user = dbSet.Find(id);
            DeleteUser(user);
        }

        public IEnumerable<User> FindUsers(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<User> query = dbSet;

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

        public User GetUserById(string id)
        {
            return dbSet.Find(id);
        }

        public void UpdadeUser(User user)
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
