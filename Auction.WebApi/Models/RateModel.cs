using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.WebApi.Models
{
    public class RateModel
    {
        [Required]
        public int TradeId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Sum { get; set; }
    }
}