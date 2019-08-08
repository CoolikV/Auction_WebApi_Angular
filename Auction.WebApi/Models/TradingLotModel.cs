using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class TradingLotModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        public int TradeDuration { get; set; }

        public string Owner { get; set; }
        public bool IsVerified { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
    }
}