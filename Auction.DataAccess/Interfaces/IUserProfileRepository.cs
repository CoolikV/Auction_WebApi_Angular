using Auction.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Auction.DataAccess.Interfaces
{
    public interface IUserProfileRepository : IDisposable
    {
        UserProfile GetProfileById(string id);
        void CreateProfile(UserProfile user);
        void UpdateProfile(UserProfile user);
        void DeleteProfile(UserProfile user);
        void DeleteProfileById(string id);
        IQueryable<UserProfile> FindProfiles(Expression<Func<UserProfile, bool>> filter = null);
    }
}
