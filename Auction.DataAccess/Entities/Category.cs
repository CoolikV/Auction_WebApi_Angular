using System.Collections.Generic;

namespace Auction.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TradingLot> TradingLots { get; set; }
        public Category()
        {
            TradingLots = new List<TradingLot>();
        }
    }
}
