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
            config.NewConfig<TradingLotDTO, TradingLot>().TwoWays();
            config.NewConfig<UserDTO, User>().TwoWays();
        }
    }
}
