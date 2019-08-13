using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        internal readonly IDataContext Database;
        internal readonly DbSet<UserProfile> dbSet;

        public UserProfileRepository(IDataContext context)
        {
            Database = context;
            dbSet = context.Set<UserProfile>();
        }

        public UserProfile GetProfileById(string id)
        {
            try
            {
                return dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserProfile GetProfileByUserName(string userName)
        {
            try
            {
                return dbSet.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateProfile(UserProfile user)
        {
            try
            {
                dbSet.Add(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProfile(UserProfile user)
        {
            try
            {
                dbSet.Attach(user);
                Database.Entry(user).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProfile(UserProfile user)
        {
            try
            {
                if (Database.Entry(user).State == EntityState.Detached)
                    dbSet.Attach(user);

                dbSet.Remove(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProfileById(string id)
        {
            try
            {
                UserProfile user = dbSet.Find(id);
                DeleteProfile(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<UserProfile> FindProfiles(Expression<Func<UserProfile, bool>> filter = null)
        {
            IQueryable<UserProfile> query = dbSet;

            return filter == null ? query : query.Where(filter);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
