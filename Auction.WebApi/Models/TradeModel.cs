using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class TradeModel
    {
        public int Id { get; set; }
        public TradingLotModel TradingLot { get; set; }
        public int DaysLeft { get; set; }
        public DateTime TradeEnd { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double LastPrice { get; set; }
    }
}