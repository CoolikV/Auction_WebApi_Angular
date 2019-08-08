using Auction.DataAccess.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Auction.DataAccess.EntityConfigurations
{
    public class TradeConfiguration : EntityTypeConfiguration<Trade>
    {
        public TradeConfiguration()
        {
            ToTable("Trades");

            Property(t => t.TradeStart)
                .IsRequired();

            Property(t => t.TradeEnd)
                .IsRequired();

            //NOT SHURE IS THAT RIGHT
            //HasOptional(t => t.LastRated)
            //    .WithOptionalDependent();
            //NOT SHURE IS THAT RIGHT

            HasRequired(t => t.TradingLot);
        }
    }
}
