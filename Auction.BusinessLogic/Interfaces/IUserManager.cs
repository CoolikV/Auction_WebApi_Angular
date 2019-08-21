using Auction.BusinessLogic.DTOs.Authorization;
using Auction.BusinessLogic.DTOs.UserProfile;
using Auction.BusinessLogic.IdentityDetails;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auction.BusinessLogic.Interfaces
{
    public interface IUserManager : IDisposable
    {
        Task<OperationDetails> CreateUserAsync(UserRegisterDTO userDto);
        IEnumerable<UserDTO> GetUsersForPage(int pageNum, int pageSize, string userName, out int pagesCount, out int totalItemsCount);
        Task<ClaimsIdentity> Authenticate(string username, string password);
        UserProfileDTO GetUserProfileById(string userId);
        Task<OperationDetails> DeleteUserAccount(string userId);
        void EditUserProfile(string userId, NewUserProfileDTO profileDto);
        UserClaimsDTO GetUserClaims(string userName);

        bool IsUserWithIdExist(string id);
        bool IsUserWithUserNameExist(string userName);
    }
}
