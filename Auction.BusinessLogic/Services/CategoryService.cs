using Auction.BusinessLogic.DTOs.Category;
using Auction.BusinessLogic.DTOs.TradingLot;
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
    /// <summary>
    /// Service for working with categories
    /// </summary>
    public class CategoryService : ICategoryService
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }

        private const int _defaultCategoryId = 1;

        public CategoryService(IUnitOfWork uow, IAdapter adapter)
        {
            Database = uow;
            Adapter = adapter;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
        
        /// <summary>
        /// Get the category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category with specified ID</returns>
        public CategoryDTO GetCategoryById(int id)
        {

            try
            {
                if(!IsCategoryExist(id))
                    throw new NotFoundException($"Category with id: {id}");

                var category = Database.Categories.GetCategoryById(id);
                return Adapter.Adapt<CategoryDTO>(category);
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
        /// <summary>
        /// Gets the list of categories
        /// </summary>
        /// <returns>List of categories</returns>
        public IEnumerable<CategoryDTO> GetCategories()
        {
            try
            {
                return Adapter.Adapt<IEnumerable<CategoryDTO>>(Database.Categories.FindCategories().AsEnumerable());
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// Get trading lot from category
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <param name="lotId">Lot ID</param>
        /// <returns>Lot</returns>
        public TradingLotDTO GetLotFromCategory(int categoryId, int lotId)
        {
            try
            {
                if (!IsCategoryContainLot(categoryId, lotId))
                    throw new NotFoundException($"Category doesn`t contain lot with id: {lotId}");

                return Adapter.Adapt<TradingLotDTO>(Database.TradingLots.FindTradingLots(l => l.Id == lotId 
                && l.CategoryId == categoryId).Single());
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
        /// <summary>
        /// Removes category
        /// </summary>
        /// <param name="id">Category ID</param>
        public void RemoveCategoryById(int id)
        {
            try
            {
                if (id == 1)
                    throw new AuctionException("You can`t delete default category");
                if(!IsCategoryExist(id))
                    throw new NotFoundException($"Category with id: {id}");

                var categoryToDelete = Database.Categories.GetCategoryById(id);

                if (categoryToDelete.TradingLots.Any())
                    MoveLotsToCategory(categoryToDelete.TradingLots);

                Database.Categories.DeleteCategoryById(categoryToDelete.Id);
                Database.Save();
            }
            catch(AuctionException ex)
            {
                throw ex;
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
        /// <summary>
        /// Changes category name
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="name">New category name</param>
        public void ChangeCategoryName(int id, string name)
        {
            try
            {
                if(!IsCategoryExist(id))
                    throw new NotFoundException($"Category with id: {id}");
                if(!IsCategoryNameFree(name))
                    throw new AuctionException($"Category with name {name} is already exists");

                var categoryToUpdate = Database.Categories.GetCategoryById(id);

                categoryToUpdate.Name = name;

                Database.Categories.UpdateCategory(categoryToUpdate);
                Database.Save();
            }
            catch(NotFoundException ex)
            {
                throw ex;
            }
            catch(AuctionException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }
        /// <summary>
        /// Creates new category
        /// </summary>
        /// <param name="category">New category</param>
        public void CreateCategory(NewCategoryDTO category)
        {
            try
            {
                if (!IsCategoryNameFree(category.Name))
                    throw new AuctionException($"Category {category.Name} already exists.");

                Database.Categories.AddCategory(new Category { Name = category.Name });
                Database.Save();
            }
            catch(AuctionException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }
        #region Condition check methods
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
        /// Сhecks is category contain lot with given id 
        /// </summary>
        /// <param name="categoryId">Category id</param>
        /// <param name="lotId">Lot Id</param>
        /// <returns>True if category with specified id contains lot with given id</returns>
        private bool IsCategoryContainLot(int categoryId, int lotId)
        {
            return Database.TradingLots.FindTradingLots(l => l.CategoryId == categoryId && l.Id == lotId).Any();
        }
        /// <summary>
        /// Change category of given lots in collection to category with given id
        /// </summary>
        /// <param name="tradingLots">Collection of lots, which category will be changed</param>
        /// <param name="categoryId">Id of new category for lots</param>
        private void MoveLotsToCategory(ICollection<TradingLot> tradingLots)
        {
            var defaultCat = Database.Categories.GetCategoryById(_defaultCategoryId);
            tradingLots.ToList().ForEach(lot => {
                lot.Category = defaultCat;
                lot.CategoryId = defaultCat.Id;
                Database.TradingLots.UpdateTradingLot(lot);
            });
        }
        #endregion
    }
}
