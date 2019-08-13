using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.IdentityDetails;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auction.BusinessLogic.Interfaces
{
    public interface IUserManager : IDisposable
    {
        Task<OperationDetails> CreateUserAsync(UserDTO userDto);
        IEnumerable<UserDTO> GetUsersForPage(int pageNum, int pageSize, string userName, out int pagesCount, out int totalItemsCount);
        Task<ClaimsIdentity> Authenticate(string username, string password);
        UserDTO GetUserByUserName(string name);
        UserDTO GetUserProfileByUserName(string userName);
        Task<OperationDetails> EditUserRoleAsync(string userId, string newRoleName);
        IEnumerable<string> GetAllRoleNames();
        Task<OperationDetails> DeleteUserAccount(string userId);
        void EditUserProfile(string userId, UserDTO user);

        bool IsUserWithUserNameExist(string userName);
    }
}
