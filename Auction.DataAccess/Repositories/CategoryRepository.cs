using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        internal readonly IDataContext Database;
        internal readonly DbSet<Category> dbSet;

        public CategoryRepository(IDataContext context)
        {
            Database = context;
            dbSet = context.Set<Category>();
        }

        public Category GetCategoryById(int id)
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

        public Category GetCategoryByName(string name)
        {
            try
            {
                return dbSet.Where(category => category.Name.Equals(name)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCategory(Category category)
        {
            try
            {
                dbSet.Add(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                dbSet.Attach(category);
                Database.Entry(category).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCategory(Category category)
        {
            try
            {
                if (Database.Entry(category).State == EntityState.Detached)
                    dbSet.Attach(category);
                dbSet.Remove(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCategoryById(int id)
        {
            try
            {
                Category category = dbSet.Find(id);
                DeleteCategory(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Category> FindCategories(Expression<Func<Category, bool>> filter = null)
        {
            IQueryable<Category> query = dbSet;

            return filter == null ? query : query.Where(filter);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
