using Auction.BusinessLogic.DTOs.Category;
using Auction.BusinessLogic.DTOs.TradingLot;
using System;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ICategoryService : IDisposable
    {
        CategoryDTO GetCategoryById(int id);
        TradingLotDTO GetLotFromCategory(int categoryId, int lotId);
        void RemoveCategoryById(int id);
        void ChangeCategoryName(int id, string name);
        void CreateCategory(NewCategoryDTO category);

        bool IsCategoryExist(int id);
    }
}
