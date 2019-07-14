using System.Collections.Generic;
using Auction.DataAccess.Identity.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.DataAccess.Entities
{
    public class User
    {
        [Key]
        [ForeignKey("AppUser")]
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<TradingLot> TradingLots { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }

        public virtual AppUser AppUser { get; set; }
        public User()
        {
            TradingLots = new List<TradingLot>();
            Trades = new List<Trade>();
        }
    }
}
