using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.DataAccess.Entities
{
    public class TradingLot
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Img { get; set; }

        public double Price { get; set; }

        public int TradeDuration { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        public bool IsVerified { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public TradingLot()
        {
            IsVerified = false;
        }
    }
}
