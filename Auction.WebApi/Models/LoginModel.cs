using System.ComponentModel.DataAnnotations;

namespace Auction.WebApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}