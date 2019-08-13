using Auction.DataAccess.Entities;
using System;
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
        IQueryable<Category> FindCategories(Expression<Func<Category, bool>> filter = null);
    }
}
