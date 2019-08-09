using Auction.BusinessLogic.DataTransfer;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        CategoryDTO GetCategoryById(int id);
        TradingLotDTO GetLotFromCategory(int categoryId, int lotId);
        IEnumerable<CategoryDTO> GetAllCategories();
        void RemoveCategoryById(int id);
        void EditCategory(int id, CategoryDTO category);
        void ChangeCategoryName(int id, string name);
        void CreateCategory(CategoryDTO category);

        bool IsCategoryExist(int id);
    }
}
