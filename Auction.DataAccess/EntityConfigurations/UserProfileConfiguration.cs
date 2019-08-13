using Auction.DataAccess.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Auction.DataAccess.EntityConfigurations
{
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
        {
            ToTable("UserProfiles");

            Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            Property(u => u.Surname)
                .HasMaxLength(50)
                .IsRequired();

            Property(u => u.UserName)
                .HasMaxLength(20)
                .IsRequired();

            Property(u => u.BirthDate)
                .IsRequired();

            HasRequired(u => u.AppUser);
        }
    }
}
