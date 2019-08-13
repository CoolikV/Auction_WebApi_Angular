using Auction.BusinessLogic.DataTransfer;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Identity.Entities;
using Mapster;
using System;

namespace Auction.BusinessLogic.Configs
{
    public class BLMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CategoryDTO, Category>().MaxDepth(3).IgnoreNullValues(true).TwoWays();

            config.NewConfig<TradeDTO, Trade>()
                .Ignore(d => d.Id)
                .Map(dest => dest.LotId, src => src.TradingLot.Id);

            config.NewConfig<Trade, TradeDTO>().MaxDepth(3);

            config.NewConfig<TradingLotDTO, TradingLot>()
                .Map(d => d.Description, s => s.Description)
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Price, s => s.Price)
                .Map(d => d.Img, s => s.Img)
                .Map(d => d.TradeDuration, s => s.TradeDuration)
                .IgnoreNonMapped(true)
                .IgnoreNullValues(true);

            config.NewConfig<TradingLot, TradingLotDTO>()
                .Map(dest => dest.Status, src => src.LotStatus);

            config.NewConfig<UserDTO, UserProfile>()
                .Ignore(dest => dest.Id);

            config.NewConfig<UserProfile, UserDTO>();

            config.NewConfig<UserDTO, AppUser>()
                .Map(d => d.Email, s => s.Email)
                .Map(d => d.UserName, s => s.UserName)
                .Map(d => d.Id, s => Guid.NewGuid().ToString())
                .IgnoreNonMapped(true);
        }
    }
}
