using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class TradingLotModel : BaseTradingLotModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Category { get; set; }
    }
}