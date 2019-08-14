using Auction.BusinessLogic.DTOs.TradingLot;
using Newtonsoft.Json;
using System;

namespace Auction.BusinessLogic.DTOs.Trade
{
    public class TradeDTO
    {
        public int Id { get; set; }

        public TradingLotDTO TradingLot { get; set; }

        [JsonIgnore]
        public DateTime TradeStart { get; set; }

        public int DaysLeft { get; set; }

        public DateTime TradeEnd { get; set; }

        [JsonIgnore]
        public string LastRateUserId { get; set; }

        public double LastPrice { get; set; }
    }
}
