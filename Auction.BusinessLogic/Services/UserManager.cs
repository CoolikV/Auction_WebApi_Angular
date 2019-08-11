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
using System;
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

                var clientProfile = Adapter.Adapt<UserProfile>(userDto);
                clientProfile.Id = user.Id;

                Database.UserProfiles.CreateProfile(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Registration successful", "");
            }
            else
            {
                return new OperationDetails(false, "User with such email already exists", "Email");
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
            return Adapter.Adapt<UserDTO>(FindUserByName(name));
        }

        public async Task EditUserRoleAsync(string userId, string newRoleName)
        {
            var appUser = await Database.UserManager.FindByIdAsync(userId)
                ?? throw new NotFoundException($"User with id: {userId}");

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

        public async Task<OperationDetails> DeleteUserAccount(string userId)
        {
            var userProfile = FindUserById(userId).UserProfile;
            var user = FindUserById(userId);

            Database.UserProfiles.DeleteProfile(userProfile);
            var operationResult = await Database.UserManager.DeleteAsync(user);

            if (operationResult.Errors.Count() > 0)
                return new OperationDetails(false, operationResult.Errors.FirstOrDefault(), "");

            await Database.SaveAsync();

            return new OperationDetails(true, $"User account with id: {userId} was successfuly deleted", "");
        }

        public UserDTO GetUserProfileById(string id)
        {
            var userProfile = FindUserById(id).UserProfile;

            return Adapter.Adapt<UserDTO>(userProfile);
        }

        public UserDTO GetUserProfileByEmail(string email)
        {
            var userProfile = FindUserByName(email).UserProfile;

            return Adapter.Adapt<UserDTO>(userProfile);
        }

        public void EditUserProfile(string id, UserDTO user)
        {
            var userProfile = FindUserById(id).UserProfile;

            userProfile = Adapter.Adapt<UserProfile>(user);

            Database.UserProfiles.UpdateProfile(userProfile);
        }

        public bool IsUserProfileExist(string userName)
        {
            return Database.UserProfiles.FindProfiles(u => u.UserName.Equals(userName)).Any();
        }
        #region Helper methods
        private AppUser FindUserByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("User name not valid", nameof(name));

            return Database.UserManager.FindByName(name)
                ?? throw new NotFoundException($"User with name {name}");
        }

        private AppUser FindUserById(string userId)
        {
            return Database.UserManager.FindById(userId)
                ?? throw new NotFoundException($"User with id: {userId}");
        }
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
