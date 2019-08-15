using Auction.BusinessLogic.DTOs.Category;
using Auction.BusinessLogic.DTOs.TradingLot;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        CategoryDTO GetCategoryById(int id);
        IEnumerable<CategoryDTO> GetCategories();
        TradingLotDTO GetLotFromCategory(int categoryId, int lotId);
        void RemoveCategoryById(int id);
        void ChangeCategoryName(int id, string name);
        void CreateCategory(NewCategoryDTO category);

        bool IsCategoryExist(int id);
    }
}
