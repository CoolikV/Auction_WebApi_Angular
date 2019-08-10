using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class RateModel
    {
        [Required(ErrorMessage = "Trade doesn`t find or already finished")]
        public int TradeId { get; set; }

        [Required(ErrorMessage = "Set the bet sum")]
        [DataType(DataType.Currency)]
        public double Sum { get; set; }
    }
}