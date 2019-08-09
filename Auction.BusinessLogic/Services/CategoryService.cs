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

        public TradingLotDTO GetLotFromCategory(int categoryId, int lotId)
        {
            var category = GetCategoryById(categoryId);

            var lotDto = category.TradingLots.FirstOrDefault(lot => lot.Id == lotId)
                ?? throw new NotFoundException($"Lot with id: {lotId} in {category.Name}");

            return lotDto;
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            return Adapter.Adapt<List<CategoryDTO>>(Database.Categories.FindCategories());
        }

        public void RemoveCategoryById(int id)
        {
            if (id == 1)
                throw new AuctionException("You can`t delete default category");

            var categoryToDelete = Database.Categories.GetCategoryById(id)
                ?? throw new NotFoundException($"Category with id: {id}");

            if (categoryToDelete.TradingLots != null)
                MoveLotsToCategory(categoryToDelete.TradingLots);

            Database.Categories.DeleteCategoryById(categoryToDelete.Id);
            Database.Save();
        }

        private void MoveLotsToCategory(ICollection<TradingLot> tradingLots, int categoryId = 1)
        {
            var defaultCat = Database.Categories.GetCategoryById(categoryId);
            tradingLots.ToList().ForEach(lot => {
                lot.Category = defaultCat;
                lot.CategoryId = defaultCat.Id;
                Database.TradingLots.UpdateTradingLot(lot);
            } );
        }

        public void EditCategory(int id, CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            var temp = Database.Categories.GetCategoryById(id) 
                ?? throw new NotFoundException($"Category with id: {id}");

            //temp.Name = category.Name;
            var updatedCategory = Adapter.Adapt<Category>(category);

            var linq = from dto in category.TradingLots
                       join db in temp.TradingLots on dto.Id equals db.Id
                       select Adapter.Adapt<TradingLot>(db);

            updatedCategory.TradingLots = linq.ToList();

            Database.Categories.UpdateCategory(updatedCategory);
            Database.Save();
        }


        public void ChangeCategoryName(int id, string name)
        {
            var temp = Database.Categories.GetCategoryById(id)
                ?? throw new NotFoundException($"Category with id: {id}");
            if(!IsCategoryNameFree(name))
                throw new AuctionException($"Category {name} already exists.");

            temp.Name = name;

            Database.Categories.UpdateCategory(temp);
            Database.Save();
        }

        //check is category with such name is already exists and add constraint to category name as unique!!!
        public void CreateCategory(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (!IsCategoryNameFree(category.Name))
                throw new AuctionException($"Category {category.Name} already exists.");

            Database.Categories.AddCategory(new Category { Name = category.Name });
            Database.Save();
        }
        //maybe change method equeals to more specified
        private bool IsCategoryNameFree(string name)
        {
            return Database.Categories.Entities.FirstOrDefault(category => category.Name.Equals(name)) is null;
        }
    }
}
