using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"^[а-яА-Я\s]+$")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[а-яА-Я\s]+$")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [RegularExpression(@"^(?=[A-Za-z0-9])(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}