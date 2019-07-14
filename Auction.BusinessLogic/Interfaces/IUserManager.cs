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
        Task<OperationDetails> Create(UserDTO userDto);
        IEnumerable<UserDTO> GetUsers();
        Task<Tuple<ClaimsIdentity, ClaimsIdentity>> FindAsync(string username, string password);
        UserDTO GetUserByName(string name);
        Task EditRole(string userId, string newRoleName);
        IEnumerable<string> GetRoles();
    }
}
