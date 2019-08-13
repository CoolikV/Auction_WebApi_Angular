using Auction.WebApi.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class RegisterModel
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

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Date)]
        [Adult(ErrorMessage = "You must be over 18 y.o. to use this service")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$",
            ErrorMessage = "Password must contain at least 1 number, 1 uppercase and 1 lowercase symbol")]
        [MaxLength(15, ErrorMessage = "Password length must be less 15 symbols")]
        [MinLength(8, ErrorMessage = "Password length must be at least 8 symbols")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Compare("Password",ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}