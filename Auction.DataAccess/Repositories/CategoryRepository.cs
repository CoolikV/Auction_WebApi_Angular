﻿using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
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

        public void AddCategory(Category category)
        {
            dbSet.Add(category);
        }

        public void DeleteCategory(Category category)
        {
            if (Database.Entry(category).State == EntityState.Detached)
                dbSet.Attach(category);

            dbSet.Remove(category);
        }

        public void DeleteCategoryById(int id)
        {
            Category category = dbSet.Find(id);
            DeleteCategory(category);
        }

        public IEnumerable<Category> FindCategory(Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Category> query = dbSet;

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

        public Category GetCategoryById(int id)
        {
            return dbSet.Find(id);
        }

        public void UpdadeCategory(Category category)
        {
            dbSet.Attach(category);
            Database.Entry(category).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
