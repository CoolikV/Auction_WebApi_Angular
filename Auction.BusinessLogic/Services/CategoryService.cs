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

        public CategoryDTO Get(int id)
        {
            return Adapter.Adapt<CategoryDTO>(Database.Categories.GetById(id));
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            return Adapter.Adapt<List<CategoryDTO>>(Database.Categories.Get());
        }

        public void RemoveCategory(int id)
        {
            if (id == 1)
                throw new AuctionException("You cant delete default category");

            var categoryToDelete = Database.Categories.GetById(id);

            if (categoryToDelete == null)
                throw new NullReferenceException();

            if (categoryToDelete.TradingLots != null)
                MoveLotsToDefaultCategory(categoryToDelete.TradingLots);

            Database.Categories.Delete(categoryToDelete.Id);
            Database.Save();
        }

        private void MoveLotsToDefaultCategory(ICollection<TradingLot> tradingLots, int defaultId = 1)
        {
            var defaultCat = Database.Categories.GetById(defaultId);
            tradingLots.ToList().ForEach(lot => lot.Category = defaultCat);
        }
        //maybe change name to ChangeCategoryName
        public void EditCategory(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var temp = Database.Categories.GetById(category.Id);

            if (temp == null)
                throw new ArgumentNullException(nameof(temp));

            temp.Name = category.Name;

            Database.Categories.Update(temp);
            Database.Save();
        }
        //check is category with such name is already exists
        public void CreateCategory(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Database.Categories.Insert(new Category { Name = category.Name });
            Database.Save();
        }
    }
}
