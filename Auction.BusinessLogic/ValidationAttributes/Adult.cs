using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.BusinessLogic.ValidationAttributes
{
    public class Adult : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateOfBirth = Convert.ToDateTime(value);
            return DateTime.Now.Year - dateOfBirth.Year >= 18;
        }
    }
}
