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
        Task EditUserRoleAsync(string userId, string newRoleName);
        IEnumerable<string> GetAllRoles();
    }
}
