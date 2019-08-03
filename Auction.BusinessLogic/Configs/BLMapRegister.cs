using Auction.BusinessLogic.DataTransfer;
using Auction.DataAccess.Entities;
using Mapster;

namespace Auction.BusinessLogic.Configs
{
    public class BLMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CategoryDTO, Category>().TwoWays();

            config.NewConfig<TradeDTO, Trade>().TwoWays();

            config.NewConfig<TradingLotDTO, TradingLot>().MaxDepth(4).IgnoreNullValues(true);
            config.NewConfig<TradingLot, TradingLotDTO>().MaxDepth(4).IgnoreNullValues(true);

            config.NewConfig<UserDTO, UserProfile>()
                .Ignore(dest => dest.Id)
                .TwoWays();

            //config.NewConfig<CategoryDTO, Category>().PreserveReference(true).TwoWays();
            //config.NewConfig<TradeDTO, Trade>().PreserveReference(true).TwoWays();
            //config.NewConfig<TradingLot, TradingLotDTO>()
            //    .IgnoreAttribute(typeof(Category))
            //    .PreserveReference(true).TwoWays();
            //config.NewConfig<UserDTO, User>().PreserveReference(true).TwoWays();
        }
    }
}
