using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }

        public CategoryService(IUnitOfWork uow, IAdapter adapter)
        {
            Database = uow;
            Adapter = adapter;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public CategoryDTO GetCategoryById(int id)
        {
            var category = Database.Categories.GetCategoryById(id)
                ?? throw new NotFoundException($"Category with id: {id}");

            return Adapter.Adapt<CategoryDTO>(category);
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            return Adapter.Adapt<List<CategoryDTO>>(Database.Categories.FindCategories());
        }

        public void RemoveCategoryById(int id)
        {
            if (id == 1)
                throw new AuctionException("You cant delete default category");

            var categoryToDelete = GetCategoryById(id);

            if (categoryToDelete == null)
                throw new NullReferenceException();

            if (categoryToDelete.TradingLots != null)
               // MoveLotsToDefaultCategory(categoryToDelete.TradingLots);

            Database.Categories.DeleteCategoryById(categoryToDelete.Id);
            Database.Save();
        }

        private void MoveLotsToDefaultCategory(ICollection<TradingLot> tradingLots, int defaultId = 1)
        {
            var defaultCat = Database.Categories.GetCategoryById(defaultId);
            tradingLots.ToList().ForEach(lot => lot.Category = defaultCat);
        }
        //maybe change name to ChangeCategoryName
        public void EditCategory(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var temp = Database.Categories.GetCategoryById(category.Id);

            if (temp == null)
                throw new ArgumentNullException(nameof(temp));

            temp.Name = category.Name;

            Database.Categories.UpdadeCategory(temp);
            Database.Save();
        }
        //check is category with such name is already exists
        public void CreateCategory(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Database.Categories.AddCategory(new Category { Name = category.Name });
            Database.Save();
        }
    }
}
