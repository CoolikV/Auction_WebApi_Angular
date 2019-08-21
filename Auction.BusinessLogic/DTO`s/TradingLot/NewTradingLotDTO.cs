using System.ComponentModel.DataAnnotations;

namespace Auction.BusinessLogic.DTOs.TradingLot
{
    public class NewTradingLotDTO
    {
        [Required(ErrorMessage = "Fill the name field", AllowEmptyStrings = false)]
        [RegularExpression(@"^[\s0-9a-zA-Z]+$")]
        [MaxLength(100)]
        [MinLength(4)]
        public string Name { get; set; }

        [RegularExpression(@"^[\s0-9a-zA-Z]+$")]
        [MaxLength(150, ErrorMessage = "Maximum 150 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image name required")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Image required")]
        public string ImgBase64 { get; set; }

        [Required(ErrorMessage = "Set the price", AllowEmptyStrings = false)]
        [DataType(DataType.Currency)]
        [Range(0, 50_000, ErrorMessage = "Set price from 0 to 50 000")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Select the category")]
        public int CategoryId { get; set; }
    }
}
