using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class StartTradeModel
    {
        [Required(ErrorMessage ="Lot id is required")]
        public int LotId { get; set; }
    }
}