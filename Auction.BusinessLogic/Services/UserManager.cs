using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.IdentityDetails;
using Auction.BusinessLogic.Interfaces;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Identity.Entities;
using Auction.DataAccess.Interfaces;
using Mapster;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auction.BusinessLogic.Services
{
    public class UserManager : IUserManager
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }

        public UserManager(IUnitOfWork uow, IAdapter adapter)
        {
            Database = uow;
            Adapter = adapter;
        }

        public async Task<OperationDetails> CreateUserAsync(UserDTO userDto)
        {
            var user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new AppUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await Database.UserManager.AddToRoleAsync(user.Id, "user");
                User clientProfile = new User { Id = user.Id, Name = userDto.UserName };
                Database.Users.AddUser(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var appUsers = Database.UserManager.Users.ToList();
            //var list = new List<UserDTO>();

            //if (appUsers != null)
            //    foreach (var appUser in appUsers)
            //        list.Add(CreateUserDTO(appUser));

            return Adapter.Adapt<IEnumerable<UserDTO>>(appUsers);
        }

        //use this mwthod above when mapping
        private UserDTO CreateUserDTO(AppUser user)
        {
            return new UserDTO()
            {
                //Id = user.Id,
                //Email = user.Email,
                //UserName = user.UserName,
                //Name = user.User.Name,
                //Role = GetRoleForUser(user.Id),
                //Lots = Mapper.Map<IEnumerable<Lot>, ICollection<LotDTO>>(DatabaseDomain.Lots.Find(x => x.User.Id == user.Id))
            };
        }

        public async Task<ClaimsIdentity> Authenticate(string userName, string password)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            var appUser = await Database.UserManager.FindAsync(userName, password)
                ?? throw new NotFoundException("The user name or password is incorrect.");

            // авторизуем его и возвращаем объект ClaimsIdentity (Bearer token)
            if (appUser != null)
                claim = await Database.UserManager.CreateIdentityAsync(appUser,
                                            OAuthDefaults.AuthenticationType);

            return claim;
        }

        public UserDTO GetUserByName(string name)
        {
            var appUser = Database.UserManager.FindByName(name) 
                ?? throw new NotFoundException();

            return Adapter.Adapt<UserDTO>(appUser);
        }

        public async Task EditUserRoleAsync(string userId, string newRoleName)
        {
            var appUser = await Database.UserManager.FindByIdAsync(userId)
                ?? throw new NotFoundException();

            var currentUserRole = GetRoleNameForUser(userId);

            if (!currentUserRole.Equals(newRoleName))
            {
                await Database.UserManager.RemoveFromRoleAsync(userId, currentUserRole);
                await Database.UserManager.AddToRoleAsync(userId, newRoleName);

                await Database.UserManager.UpdateAsync(appUser);
            }
        }

        public IEnumerable<string> GetAllRoles()
        {
            return Database.UserRoleManager.Roles.Select(r => r.Name);
        }

        private string GetRoleNameForUser(string userId)
        {
            var user = Database.UserManager.FindById(userId);
            var roleId = user.Roles.Where(x => x.UserId == user.Id).Single().RoleId;
            var role = Database.UserRoleManager.Roles.Where(x => x.Id == roleId).Single().Name;

            return role;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

    }
}
