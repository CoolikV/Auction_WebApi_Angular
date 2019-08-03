using System.Collections.Generic;

namespace Auction.WebApi.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public ICollection<TradingLotModel> TradingLots { get; set; }
    }
}