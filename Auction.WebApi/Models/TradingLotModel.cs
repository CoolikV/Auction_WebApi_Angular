using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class TradingLotModel
    {
        public int Id { get; set; }
        //Add validation
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Img { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        public int TradeDuration { get; set; }

        [Required]
        public string Owner { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}