using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Set category name")]
        [RegularExpression(@"^[A-Z,&/a-z\s?]{4,50}$", ErrorMessage = "Category name can only be letters")]
        [MinLength(4, ErrorMessage = "Category name must be at least 4 characters")]
        [MaxLength(50, ErrorMessage = "Category name must be less than 50 characters")]
        public string Name { get; set; }
    }
}