using Auction.BusinessLogic.DataTransfer;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        CategoryDTO GetCategoryById(int id);
        IEnumerable<CategoryDTO> GetAllCategories();
        void RemoveCategoryById(int id);
        void EditCategory(CategoryDTO category);
        void CreateCategory(CategoryDTO category);
    }
}
