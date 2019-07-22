using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Auction.BusinessLogic.DataTransfer;
using Auction.DataAccess.Entities;

namespace Auction.BusinessLogic.Configs
{
    public class BLRegister : IRegister
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
