﻿using Auction.BusinessLogic.DTOs.TradingLot;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Entities.Enums;
using Auction.DataAccess.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.BusinessLogic.Services
{
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

        public void Dispose()
        {
            Database.Dispose();
        }

        public void CreateLot(NewTradingLotDTO lot, string userName)
        {
            var lotPoco = Adapter.Adapt<TradingLot>(lot);
            try
            {
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

        //delete old image from app data and save new image, then set new img path
        public void EditLot(int lotId, NewTradingLotDTO lotDto, bool isManager)
        {
            try
            {
                if (!IsLotExists(lotId))
                    throw new NotFoundException($"Lot with id: {lotId}");

                TradingLot lotPoco = Database.TradingLots.GetTradingLotById(lotId);
                if (lotPoco.LotStatus == LotStatus.OnSale)
                    throw new AuctionException("You can`t change the information about the lot after the start of the bidding");
                if (isManager && lotPoco.CategoryId != lotDto.CategoryId)
                {
                    if (CategoryService.IsCategoryExist(lotDto.CategoryId) )
                    {
                        lotPoco.CategoryId = lotDto.CategoryId;
                        lotPoco.Category = Database.Categories.GetCategoryById(lotDto.CategoryId);
                    }
                }

                lotDto.Adapt(lotPoco);

                Database.TradingLots.UpdateTradingLot(lotPoco);
                Database.Save();
            }
            catch(AuctionException ex)
            {
                throw ex;
            }
            catch(Exception)
            {
                throw new DatabaseException();
            }
        }

        public void RemoveLotById(int lotId)
        {
            try
            {
                if (!IsLotExists(lotId))
                    throw new NotFoundException();

                Database.TradingLots.DeleteTradingLotById(lotId);
                Database.Save();
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

        public TradingLotDTO GetLotById(int lotId)
        {
            if (!IsLotExists(lotId))
                throw new NotFoundException();
            try
            {
                return Adapter.Adapt<TradingLotDTO>(Database.TradingLots.GetTradingLotById(lotId));
            }
            catch(Exception)
            {
                throw new DatabaseException();
            }
        }

        public void ChangeLotCategory(int lotId, int categoryId)
        {
            if (!IsLotExists(lotId) || !CategoryService.IsCategoryExist(categoryId))
                throw new NotFoundException();
            try
            {
                TradingLot lot = Database.TradingLots.GetTradingLotById(lotId);
                Category category = Database.Categories.GetCategoryById(categoryId);
                lot.Category = category;
                lot.CategoryId = categoryId;
                Database.TradingLots.UpdateTradingLot(lot);
                Database.Save();
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
        }

        public IEnumerable<TradingLotDTO> GetLotsForPage(int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterLotsForPage(string.Empty, pageNum, pageSize, categoryId, minPrice, maxPrice, lotName, out pagesCount, out totalItemsCount);
        }

        public IEnumerable<TradingLotDTO> GetLotsForUser(string userId, int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            return FilterLotsForPage(userId, pageNum, pageSize, categoryId, minPrice, maxPrice, lotName, out pagesCount, out totalItemsCount);
        }

        private IEnumerable<TradingLotDTO> FilterLotsForPage(string userId, int pageNum, int pageSize, int? categoryId,
            double? minPrice, double? maxPrice, string lotName, out int pagesCount, out int totalItemsCount)
        {
            IQueryable<TradingLot> source = Database.TradingLots.FindTradingLots();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                if(!UserManager.IsUserWithIdExist(userId))
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

            totalItemsCount = source.Count();

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var lotsForPage = source.OrderBy(l => l.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<TradingLotDTO>>(lotsForPage);
        }

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
    }
}