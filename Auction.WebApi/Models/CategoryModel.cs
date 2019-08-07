using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z,&/a-z\s?]{4,50}$")]
        public string Name { get; set; }

        public ICollection<TradingLotModel> TradingLots { get; set; }
    }
}