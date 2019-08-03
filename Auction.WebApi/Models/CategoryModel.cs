using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[А-Я,/а-я\s?]{4,25}$")]
        public string Name { get; set; }

        public ICollection<TradingLotModel> TradingLots { get; set; }
    }
}