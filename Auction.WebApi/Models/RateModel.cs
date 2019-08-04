using System.ComponentModel.DataAnnotations;

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