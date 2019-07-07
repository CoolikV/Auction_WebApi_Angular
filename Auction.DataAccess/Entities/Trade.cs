using System;

namespace Auction.DataAccess.Entities
{
    public class Trade
    {
        public int Id { get; set; }

        public virtual TradingLot TradingLot { get; set; }
        public int LotId { get; set; }

        public DateTime TradeStart { get; set; }

        public DateTime TradeEnd { get; set; }

        public string LastRateUserId { get; set; }

        public double LastPrice { get; set; }
    }
}
