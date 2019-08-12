﻿using Auction.BusinessLogic.DataTransfer;
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
            if (IsUserWithEmailExist(userDto.Email))
                return new OperationDetails(false, "User with such email already exists, please log in", nameof(userDto.Email));
            if(IsUserWithUserNameExist(userDto.UserName))
                return new OperationDetails(false, $"User name {userDto.UserName} is already taken, please take another", nameof(userDto.UserName));
            //add mapping and ignore not mapped
            AppUser user = new AppUser { Email = userDto.Email, UserName = userDto.UserName };
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

            return new OperationDetails(true, "Registration successful", string.Empty);
        }

        public IEnumerable<UserDTO> GetUsersForPage(int pageNum, int pageSize, string name, out int pagesCount, out int totalItemsCount)
        {
            IQueryable<AppUser> source = Database.UserManager.Users;
            if (!string.IsNullOrWhiteSpace(name))
                source = source.Where(user => user.UserName.ToLower().Contains(name.ToLower()));

            totalItemsCount = source.Count();
            if (totalItemsCount < 1)
                throw new NotFoundException();

            pagesCount = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            var usersForPage = source.OrderBy(l => l.UserName)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            return Adapter.Adapt<IEnumerable<UserDTO>>(usersForPage);
        }

        public async Task<ClaimsIdentity> Authenticate(string userName, string password)
        {
            var appUser = await Database.UserManager.FindAsync(userName, password)
                ?? throw new NotFoundException("The user name or password is incorrect");

            ClaimsIdentity claim = await Database.UserManager.CreateIdentityAsync(appUser, OAuthDefaults.AuthenticationType);

            return claim;
        }

        public UserDTO GetUserByUserName(string userName)
        {
            if (!IsUserWithUserNameExist(userName))
                throw new NotFoundException($"User with user name: {userName}");

            return Adapter.Adapt<UserDTO>(Database.UserManager.FindByName(userName));
        }
        //maybe change
        public async Task<OperationDetails> EditUserRoleAsync(string userId, string newRoleName)
        {
            if (!IsUserByIdExist(userId))
                throw new NotFoundException("Selected user");
            var appUser = await Database.UserManager.FindByIdAsync(userId);

            string currentUserRole = GetRoleNameForUser(appUser);

            if (currentUserRole.Equals(newRoleName))
                return new OperationDetails(false,"User already in this role", nameof(newRoleName));
            
            await Database.UserManager.RemoveFromRoleAsync(userId, currentUserRole);
            await Database.UserManager.AddToRoleAsync(userId, newRoleName);

            await Database.UserManager.UpdateAsync(appUser);

            return new OperationDetails(true, $"Role for user {appUser.UserName} was changed from {currentUserRole} to {newRoleName}", string.Empty);
        }

        public async Task<OperationDetails> DeleteUserAccount(string userName)
        {
            if (!IsUserWithUserNameExist(userName))
                throw new NotFoundException($"User with name: {userName}");
            //var userProfile = FindUserById(userId).UserProfile;

            //Database.UserProfiles.DeleteProfile(userProfile);
            var operationResult = await Database.UserManager.DeleteAsync(Database.UserManager.FindByName(userName));

            if (operationResult.Errors.Any())
                return new OperationDetails(false, operationResult.Errors.FirstOrDefault(), "");

            await Database.SaveAsync();

            return new OperationDetails(true, $"User account {userName} was successfuly deleted", string.Empty);
        }

        public UserDTO GetUserProfileByUserName(string userName)
        {
            if (!IsUserWithUserNameExist(userName))
                throw new NotFoundException($"User with user name: {userName}");

            return Adapter.Adapt<UserDTO>(Database.UserManager.FindByName(userName).UserProfile);
        }

        public void EditUserProfile(string userId, UserDTO userDto)
        {
            if (!IsUserByIdExist(userId))
                throw new NotFoundException($"User with id: {userId}");

            try
            {
                var userProfile = Database.UserProfiles.GetProfileById(userId);
                userProfile = Adapter.Adapt<UserProfile>(userDto);

                Database.UserProfiles.UpdateProfile(userProfile);
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }
            Database.Save();
        }

        public IEnumerable<string> GetAllRoleNames()
        {
            return Database.UserRoleManager.Roles.Select(r => r.Name);
        }

        private string GetRoleNameForUser(AppUser user)
        {
            var roleId = user.Roles.Where(role => role.UserId == user.Id).Single().RoleId;
            var roleName = Database.UserRoleManager.Roles.Where(role => role.Id == roleId).Single().Name;

            return roleName;
        }
        
        #region Condition check methods
        public bool IsUserWithUserNameExist(string userName)
        {
            return Database.UserManager.FindByName(userName) != null;
        }

        private bool IsUserWithEmailExist(string email)
        {
            return Database.UserManager.FindByEmail(email) != null;
        }

        private bool IsUserByIdExist(string id)
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
