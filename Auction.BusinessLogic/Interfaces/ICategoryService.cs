using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        CategoryDTO Get(int id);
        IEnumerable<CategoryDTO> GetAll();
        void RemoveCategory(int id);
        void EditCategory(CategoryDTO category);
        void CreateCategory(CategoryDTO category);
        void Dispose();
    }
}
