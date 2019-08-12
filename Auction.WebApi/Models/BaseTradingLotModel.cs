using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class BaseTradingLotModel
    {
        [Required(ErrorMessage = "Fill in the name field", AllowEmptyStrings = false)]
        [RegularExpression(@"^[\s0-9a-zA-Z]+$")]
        [MaxLength(100)]
        [MinLength(4)]
        public string Name { get; set; }

        [RegularExpression(@"^[\s0-9a-zA-Z]+$")]
        [MaxLength(150, ErrorMessage = "Maximum 150 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Img { get; set; }  

        [Required(ErrorMessage = "Upload the lot picture")]
        public byte[] ImgBytes { get; set; }

        [Required(ErrorMessage = "Set the price", AllowEmptyStrings = false)]
        [DataType(DataType.Currency)]
        [Range(0, 50_000, ErrorMessage = "Set price from 0 to 50 000")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Select how long you want to put lot on a sale")]
        [Range(1, 31, ErrorMessage = "Salect value from 1 to 31 (days)")]
        public int TradeDuration { get; set; }

        [Required(ErrorMessage = "Select the category")]
        public int CategoryId { get; set; }
    }
}