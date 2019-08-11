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
        IEnumerable<UserDTO> GetAllUsers();
        Task<ClaimsIdentity> Authenticate(string username, string password);
        UserDTO GetUserByName(string name);
        UserDTO GetUserProfileById(string userId);
        UserDTO GetUserProfileByEmail(string email);
        Task EditUserRoleAsync(string userId, string newRoleName);
        IEnumerable<string> GetAllRoles();
        Task<OperationDetails> DeleteUserAccount(string userId);
        void EditUserProfile(string userId, UserDTO user);

        bool IsUserProfileExist(string userName);
    }
}
