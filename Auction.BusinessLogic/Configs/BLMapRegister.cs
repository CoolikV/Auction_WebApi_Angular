using Auction.BusinessLogic.DataTransfer;
using Auction.DataAccess.Entities;
using Mapster;

namespace Auction.BusinessLogic.Configs
{
    public class BLMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CategoryDTO, Category>().MaxDepth(3).IgnoreNullValues(true).TwoWays();
            //maybe check configs for other entities
            config.NewConfig<TradeDTO, Trade>()
                .Ignore(d => d.Id)
                .Map(dest => dest.LotId, src => src.TradingLot.Id);

            config.NewConfig<Trade, TradeDTO>().MaxDepth(3);
                //.IgnoreIf((src, dest) => !string.IsNullOrEmpty(src.LastRateUserId), dest => dest.LastRateUserId);
                //.Ignore(dest => dest.LastRateUserId);

            config.NewConfig<TradingLotDTO, TradingLot>().MaxDepth(4).IgnoreNullValues(true);
            config.NewConfig<TradingLot, TradingLotDTO>()
                .Map(dest => dest.Status, src => src.LotStatus)
                .MaxDepth(5).IgnoreNullValues(true);

            config.NewConfig<UserDTO, UserProfile>()
                .Ignore(dest => dest.Id);

            config.NewConfig<UserProfile, UserDTO>();

            //config.NewConfig<CategoryDTO, Category>().PreserveReference(true).TwoWays();
            //config.NewConfig<TradeDTO, Trade>().PreserveReference(true).TwoWays();
            //config.NewConfig<TradingLot, TradingLotDTO>()
            //    .IgnoreAttribute(typeof(Category))
            //    .PreserveReference(true).TwoWays();
            //config.NewConfig<UserDTO, User>().PreserveReference(true).TwoWays();
        }
    }
}
