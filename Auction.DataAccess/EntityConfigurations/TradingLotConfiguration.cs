using Auction.DataAccess.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Auction.DataAccess.EntityConfigurations
{
    public class TradingLotConfiguration : EntityTypeConfiguration<TradingLot>
    {
        public TradingLotConfiguration()
        {
            ToTable("TradingLots");

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Description)
                .IsOptional()
                .HasMaxLength(150);
           
            Property(t => t.Img)
                .IsRequired();
           
            Property(t => t.Price)
                .IsRequired();
      
            HasRequired(t => t.Category);
   
            HasRequired(t => t.User);
        }
    }
}
