using Auction.BusinessLogic.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.BusinessLogic.DTOs.UserProfile
{
    public class NewUserProfileDTO
    {
        [Required(ErrorMessage = "Required field")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name cannot contain numbers and special symbols")]
        [MaxLength(50, ErrorMessage = "Name must be less 50 symbols")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required field")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Surname cannot contain numbers and special symbols")]
        [MaxLength(50, ErrorMessage = "Surname must be less 50 symbols")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Required field")]
        [RegularExpression(@"^(?=[A-Za-z0-9])(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$",
            ErrorMessage = "Username cannot contain special characters(@, #, %, & etc...)")]
        [MaxLength(20, ErrorMessage = "Username must be less 20 symbols")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 symbols")]
        public string UserName { get; set; }
    }
}
