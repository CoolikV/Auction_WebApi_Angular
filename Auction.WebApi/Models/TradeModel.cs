using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class TradeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Trade cant be without trading lot")]
        public TradingLotModel TradingLot { get; set; }

        public int DaysLeft { get; set; }

        public DateTime TradeEnd { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Currency)]
        public double LastPrice { get; set; }
    }
}