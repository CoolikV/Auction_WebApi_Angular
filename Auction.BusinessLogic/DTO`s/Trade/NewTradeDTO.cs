using System.ComponentModel.DataAnnotations;

namespace Auction.BusinessLogic.DTOs.Trade
{
    public class NewTradeDTO
    {
        [Required(ErrorMessage = "Lot id is required")]
        public int LotId { get; set; }
    }
}
