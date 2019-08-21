using Auction.BusinessLogic.DTOs.TradingLot;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Auction.BusinessLogic.Services
{
    /// <summary>
    /// Service for working with trading lots
    /// </summary>
    public class TradingLotService : ITradingLotService
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }
        ICategoryService CategoryService { get; set; }
        IUserManager UserManager { get; set; }

        public TradingLotService(IUnitOfWork uow, IAdapter adapter, ICategoryService categoryService, IUserManager userManager)
        {
            Database = uow;
            Adapter = adapter;
            CategoryService = categoryService;
            UserManager = userManager;
        }

        /// <summary>
        /// Creates new lot
        /// </summary>
        /// <param name="lot">New lot</param>
        /// <param name="userName">User name who creates new lot</param>
        /// <param name="folder">Folder where to save lot picture</param>
        public void CreateLot(NewTradingLotDTO lot, string userName, string folder)
        {
            var lotPoco = Adapter.Adapt<TradingLot>(lot);
            try
            {
                var fileName = $@"{DateTime.Now.Ticks}{lot.Img}";

                SaveImageToFolder(lot.ImgBase64, fileName, folder);
                lotPoco.Img = fileName;
                lotPoco.User = Database.UserProfiles.GetProfileByUserName(userName);
                lotPoco.Category = Database.Categories.GetCategoryById(lot.CategoryId);

                Database.TradingLots.AddTradingLot(lotPoco);
                Database.Save();
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// Removes lot
        /// </summary>
        /// <param name="lotId">Lot ID</param>
        public void RemoveLotById(int lotId)
        {
            try
            {
                if (!IsLotExists(lotId))
                    throw new NotFoundException();

                Database.TradingLots.DeleteTradingLotById(lotId);
                Database.Save();
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// Gets lot
        /// </summary>
        /// <param name="lotId">Lot ID</param>
        /// <returns>Lot</returns>
        public TradingLotDTO GetLotById(int lotId)
        {
            if (!IsLotExists(lotId))
                throw new NotFoundException();
            try
            {
                return Adapter.Adapt<TradingLotDTO>(Database.TradingLots.GetTradingLotById(lotId));
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// Get lots with filtering and pagination
        /// </summary>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="minPrice">Min lot price</param>
        /// <param name="maxPrice">Max lot price</param>
        /// <param name="lotName">Lot name</param>
        /// <param name="pagesCount">Pages count</param>
        /// <param name="totalItemsCount">Total items found</param>
        /// <returns>Filtered collection of lots</returns>
        public IEnumerable<TradingLotDTO> GetLotsForPage(int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterLotsForPage(string.Empty, pageNum, pageSize, categoryId, minPrice, maxPrice, lotName, out pagesCount, out totalItemsCount);
        }

        /// <summary>
        /// Get lots with filtering and pagination for user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="minPrice">Min lot price</param>
        /// <param name="maxPrice">Max lot price</param>
        /// <param name="lotName">Lot name</param>
        /// <param name="pagesCount">Pages count</param>
        /// <param name="totalItemsCount">Total items found</param>
        /// <returns>Filtered collection of lots</returns>
        public IEnumerable<TradingLotDTO> GetLotsForUser(string userId, int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterLotsForPage(userId, pageNum, pageSize, categoryId, minPrice, maxPrice, lotName, out pagesCount, out totalItemsCount);
        }

        /// <summary>
        /// Filter lots for page
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="minPrice">Min lot price</param>
        /// <param name="maxPrice">Max lot price</param>
        /// <param name="lotName">Lot name</param>
        /// <param name="pagesCount">Pages count</param>
        /// <param name="totalItemsCount">Total items found</param>
        /// <returns>Filtered collection of lots</returns>
        private IEnumerable<TradingLotDTO> FilterLotsForPage(string userId, int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            IQueryable<TradingLot> source = Database.TradingLots.FindTradingLots();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                if (!UserManager.IsUserWithIdExist(userId))
                    throw new AuctionException("User with this id does`t exist, check user id and try again");
                source = source.Where(l => l.UserId == userId);
            }

            if (categoryId.HasValue && CategoryService.IsCategoryExist(categoryId.Value))
                source = source.Where(l => l.CategoryId == categoryId);

            if (maxPrice.HasValue)
                if (minPrice < maxPrice)
                    source = source.Where(l => l.Price >= minPrice && l.Price <= maxPrice);
                else
                    source = source.Where(l => l.Price <= maxPrice);
            else
                source = source.Where(l => l.Price >= minPrice);

            if (!string.IsNullOrWhiteSpace(lotName))
                source = source.Where(l => l.Name.ToLower().Contains(lotName.ToLower()));

            try
            {
                totalItemsCount = source.Count();
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var lotsForPage = source.OrderBy(l => l.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(lotsForPage);
        }

        /// <summary>
        /// Checks is lot exists
        /// </summary>
        /// <param name="id">Lot ID</param>
        /// <returns>True if lot with specified ID exists</returns>
        public bool IsLotExists(int id)
        {
            try
            {
                return Database.TradingLots.FindTradingLots(l => l.Id == id).Any();
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        /// <summary>
        /// Saves file to specified folder
        /// </summary>
        /// <param name="base64String">File presentation in base64 format</param>
        /// <param name="fileName">File name</param>
        /// <param name="folder">Folder path</param>
        private void SaveImageToFolder(string base64String, string fileName, string folder)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                using (var imageFile = new FileStream(folder + fileName, FileMode.Create))
                {
                    imageFile.Write(imageBytes, 0, imageBytes.Length);
                    imageFile.Flush();
                }
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}