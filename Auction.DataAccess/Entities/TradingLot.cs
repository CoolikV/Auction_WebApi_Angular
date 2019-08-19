using Auction.DataAccess.Entities.Enums;

namespace Auction.DataAccess.Entities
{
    public class TradingLot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public int TradeDuration { get; set; }
        public LotStatus LotStatus { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string UserId { get; set; }
        public virtual UserProfile User { get; set; }

        public TradingLot()
        {
            LotStatus = LotStatus.NotOnSale;
        }
    }
}
