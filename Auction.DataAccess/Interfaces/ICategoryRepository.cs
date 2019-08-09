using Auction.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Interfaces
{
    public interface ICategoryRepository : IDisposable
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategoryById(int id);
        void DeleteCategory(Category category);
        IEnumerable<Category> FindCategories(Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null);
        IQueryable<Category> Entities { get; }
    }
}
