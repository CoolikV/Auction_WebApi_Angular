using System;

namespace Auction.BusinessLogic.DTOs.UserProfile
{
    public class UserProfileDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
