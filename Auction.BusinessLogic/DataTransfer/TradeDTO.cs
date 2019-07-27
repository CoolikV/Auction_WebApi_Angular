using System;

namespace Auction.BusinessLogic.DataTransfer
{
    public class TradeDTO
    {
        public int Id { get; set; }
        public TradingLotDTO TradingLot { get; set; }
        public DateTime TradeStart { get; set; }
        public DateTime TradeEnd { get; set; }
        public string LastRateUserId { get; set; }
        public double LastPrice { get; set; }
    }
}
