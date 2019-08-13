using System;

namespace Auction.WebApi.Models
{
    public class UserProfileModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}