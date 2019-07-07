using System.Collections.Generic;

namespace Auction.DataAccess.Entities
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<TradingLot> TradingLots { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }

        public User()
        {
            TradingLots = new List<TradingLot>();
            Trades = new List<Trade>();
        }
    }
}
