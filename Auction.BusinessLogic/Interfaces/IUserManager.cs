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
        Task<OperationDetails> CreateUser(UserDTO userDto);
        IEnumerable<UserDTO> GetAllUsers();
        Task<Tuple<ClaimsIdentity, ClaimsIdentity>> FindUserAsync(string username, string password);
        UserDTO GetUserByName(string name);
        Task EditUserRole(string userId, string newRoleName);
        IEnumerable<string> GetAllRoles();
    }
}
