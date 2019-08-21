using Auction.BusinessLogic.DTOs.Authorization;
using Auction.BusinessLogic.DTOs.UserProfile;
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
    /// <summary>
    /// Service for working with users
    /// </summary>
    public class UserManager : IUserManager
    {
        IAdapter Adapter { get; set; }
        IUnitOfWork Database { get; set; }

        public UserManager(IUnitOfWork uow, IAdapter adapter)
        {
            Database = uow;
            Adapter = adapter;
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="userDto">New user</param>
        /// <returns>Operation details which contains is operation was successful</returns>
        public async Task<OperationDetails> CreateUserAsync(UserRegisterDTO userDto)
        {
            if (IsUserWithEmailExist(userDto.Email))
                return new OperationDetails(false, "User with such email already exists, please log in", nameof(userDto.Email));
            if(IsUserWithUserNameExist(userDto.UserName))
                return new OperationDetails(false, $"User name {userDto.UserName} is already taken, please take another", nameof(userDto.UserName));
            try
            {
                AppUser user = Adapter.Adapt<AppUser>(userDto);
                var createUserResult = await Database.UserManager.CreateAsync(user, userDto.Password);

                if (createUserResult.Errors.Any()) 
                    return new OperationDetails(false, createUserResult.Errors.FirstOrDefault(), string.Empty);

                var addToRoleResult = await Database.UserManager.AddToRoleAsync(user.Id, "user");

                if (addToRoleResult.Errors.Any())
                    return new OperationDetails(false, addToRoleResult.Errors.FirstOrDefault(), string.Empty);

                var clientProfile = Adapter.Adapt<UserProfile>(userDto);
                clientProfile.Id = user.Id;

                Database.UserProfiles.CreateProfile(clientProfile);
                await Database.SaveAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

            return new OperationDetails(true, "Registration successful", string.Empty);
        }

        /// <summary>
        /// Get users with pagination and filtering
        /// </summary>
        /// <param name="pageNum">Page number</param>
        /// <param name="pageSize">Users per page</param>
        /// <param name="userName">User name</param>
        /// <param name="pagesCount">Pages count</param>
        /// <param name="totalItemsCount">Users found</param>
        /// <returns>Filtered collection of users</returns>
        public IEnumerable<UserDTO> GetUsersForPage(int pageNum, int pageSize, string userName, out int pagesCount, out int totalItemsCount)
        {
            IQueryable<UserProfile> source = Database.UserProfiles.FindProfiles();
            if (!string.IsNullOrWhiteSpace(userName))
                source = source.Where(user => user.UserName.ToLower().Contains(userName.ToLower()));

            totalItemsCount = source.Count();

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var usersForPage = source.OrderBy(l => l.UserName)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<UserDTO>>(usersForPage);
        }

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">user password</param>
        /// <returns>Claims for user</returns>
        public async Task<ClaimsIdentity> Authenticate(string userName, string password)
        {
            var appUser = await Database.UserManager.FindAsync(userName, password)
                ?? throw new NotFoundException("The user name or password is incorrect");

            ClaimsIdentity claim = await Database.UserManager.CreateIdentityAsync(appUser, OAuthDefaults.AuthenticationType);

            return claim;
        }

        /// <summary>
        /// Gets user profile
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User profile</returns>
        public UserProfileDTO GetUserProfileById(string userId)
        {
            if (!IsUserWithIdExist(userId))
                throw new NotFoundException($"User with id: {userId}");
            return Adapter.Adapt<UserProfileDTO>(Database.UserManager.FindById(userId).UserProfile);
        }
     
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Information about operation</returns>
        public async Task<OperationDetails> DeleteUserAccount(string userName)
        {
            if (!IsUserWithUserNameExist(userName))
                throw new NotFoundException($"User with name: {userName}");

            var operationResult = await Database.UserManager.DeleteAsync(Database.UserManager.FindByName(userName));

            if (operationResult.Errors.Any())
                return new OperationDetails(false, operationResult.Errors.FirstOrDefault(), "");

            await Database.SaveAsync();

            return new OperationDetails(true, $"User account {userName} was successfuly deleted", string.Empty);
        }

        /// <summary>
        /// Edits user profile
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="profileDto">Edited user profile</param>
        public void EditUserProfile(string userId, NewUserProfileDTO profileDto)
        {
            if (!IsUserWithIdExist(userId))
                throw new NotFoundException($"User with id: {userId}");

            try
            {
                var userProfile = Database.UserProfiles.GetProfileById(userId);
                userProfile = Adapter.Adapt<UserProfile>(profileDto);

                Database.UserProfiles.UpdateProfile(userProfile);
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
            Database.Save();
        }

        /// <summary>
        /// Gets user claims
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Claims for user</returns>
        public UserClaimsDTO GetUserClaims(string userName)
        {
            try
            {
                var user = Database.UserManager.FindByName(userName);

                var userClaims = Adapter.Adapt<UserClaimsDTO>(user);

                userClaims.Role = GetRoleNameForUser(user);

                return userClaims;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets user role
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User role</returns>
        private string GetRoleNameForUser(AppUser user)
        {
            var roleId = user.Roles.Where(x => x.UserId == user.Id).Single().RoleId;
            var role = Database.UserRoleManager.Roles.Where(x => x.Id == roleId).Single().Name;

            return role;
        }
        
        #region Condition check methods
        /// <summary>
        /// Checks is user with specified user name exists
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>True if user wxists</returns>
        public bool IsUserWithUserNameExist(string userName)
        {
            return Database.UserManager.FindByName(userName) != null;
        }

        /// <summary>
        /// Checks is user with specified email exists
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>True if user exists</returns>
        private bool IsUserWithEmailExist(string email)
        {
            return Database.UserManager.FindByEmail(email) != null;
        }

        /// <summary>
        /// Checks is user with specified ID exists
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>True id user exists</returns>
        public bool IsUserWithIdExist(string id)
        {
            return Database.UserManager.FindById(id) != null;
        }
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
