using System.ComponentModel.DataAnnotations;

namespace Auction.BusinessLogic.DTOs.Trade
{
    public class RateDTO
    {
        [Required(ErrorMessage = "Trade is required")]
        public int TradeId { get; set; }

        [Required(ErrorMessage = "Set the bet sum")]
        [DataType(DataType.Currency)]
        public double Sum { get; set; }
    }
}
