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

            HasRequired(t => t.TradingLot);
        }
    }
}
