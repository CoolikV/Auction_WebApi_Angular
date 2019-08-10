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
            if(!IsCategoryExist(id))
                throw new NotFoundException($"Category with id: {id}");

            try
            {
                var category = Database.Categories.GetCategoryById(id);
                return Adapter.Adapt<CategoryDTO>(category);
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        public TradingLotDTO GetLotFromCategory(int categoryId, int lotId)
        {
            try
            {
                CategoryDTO category = GetCategoryById(categoryId);
                TradingLotDTO tradingLot = category.TradingLots.FirstOrDefault(lot => lot.Id == lotId)
                    ?? throw new NotFoundException($"Lot with id: {lotId} in {category.Name}");

                return tradingLot;
            }
            catch(NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        public void RemoveCategoryById(int id)
        {
            if (id == 1)
                throw new AuctionException("You can`t delete default category");
            if(!IsCategoryExist(id))
                throw new NotFoundException($"Category with id: {id}");

            try
            {
                var categoryToDelete = Database.Categories.GetCategoryById(id);

                if (categoryToDelete.TradingLots.Any())
                    MoveLotsToCategory(categoryToDelete.TradingLots);

                Database.Categories.DeleteCategoryById(categoryToDelete.Id);
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
            Database.Save();
        }

        //doesn`t work correctly remove or change
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
            if(!IsCategoryExist(id))
                throw new NotFoundException($"Category with id: {id}");
            if(!IsCategoryNameFree(name))
                throw new AuctionException($"Category {name} already exists.");

            try
            {
                var temp = Database.Categories.GetCategoryById(id);

                temp.Name = name;

                Database.Categories.UpdateCategory(temp);
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
            Database.Save();
        }

        public void CreateCategory(CategoryDTO category)
        {
            if (!IsCategoryNameFree(category.Name))
                throw new AuctionException($"Category {category.Name} already exists.");

            try
            {
                Database.Categories.AddCategory(new Category { Name = category.Name });
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
            Database.Save();
        }

        /// <summary>
        /// Сhecks if there is a category with given name in the database 
        /// </summary>
        /// <param name="name">Name of the category</param>
        /// <returns>True if there is no category with given name</returns>
        private bool IsCategoryNameFree(string name)
        {
            return !Database.Categories.FindCategories(category => category.Name.Equals(name)).Any();
        }
        /// <summary>
        /// Сhecks if there is a category with given id in the database 
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>True if there is category with given id</returns>
        public bool IsCategoryExist(int id)
        {
            return Database.Categories.FindCategories(c => c.Id.Equals(id)).Any();
        }
        /// <summary>
        /// Change category of given lots in collection to category with given id
        /// </summary>
        /// <param name="tradingLots">Collection of lots, which category will be changed</param>
        /// <param name="categoryId">Id of new category for lots</param>
        private void MoveLotsToCategory(ICollection<TradingLot> tradingLots, int categoryId = 1)
        {
            var defaultCat = Database.Categories.GetCategoryById(categoryId);
            tradingLots.ToList().ForEach(lot => {
                lot.Category = defaultCat;
                lot.CategoryId = defaultCat.Id;
                Database.TradingLots.UpdateTradingLot(lot);
            });
        }
    }
}
