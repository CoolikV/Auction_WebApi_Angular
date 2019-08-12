namespace Auction.WebApi.Models
{
    public class TradingLotModel : BaseTradingLotModel
    {
        public int Id { get; set; }
        
        public string Owner { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }
    }
}