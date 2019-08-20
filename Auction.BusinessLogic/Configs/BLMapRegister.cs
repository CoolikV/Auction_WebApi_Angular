using Auction.BusinessLogic.DTOs.Authorization;
using Auction.BusinessLogic.DTOs.Category;
using Auction.BusinessLogic.DTOs.Trade;
using Auction.BusinessLogic.DTOs.TradingLot;
using Auction.BusinessLogic.DTOs.UserProfile;
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

            config.NewConfig<Trade, TradeDTO>()
                .Map(d => d.DaysLeft, src => src.TradeEnd.Subtract(DateTime.Now).Days)
                .MaxDepth(3);

            config.NewConfig<TradingLotDTO, TradingLot>()
                .Map(d => d.Description, s => s.Description)
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Price, s => s.Price)
                .Map(d => d.Img, s => s.Img)
                .IgnoreNonMapped(true)
                .IgnoreNullValues(true);

            config.NewConfig<TradingLot, TradingLotDTO>()
                .Map(d => d.Owner, src => src.User.UserName);

            config.NewConfig<UserDTO, UserProfile>()
                .Ignore(dest => dest.Id);

            config.NewConfig<UserProfile, UserDTO>()
                .Map(d => d.Email, s => s.AppUser.Email);

            config.NewConfig<UserRegisterDTO, AppUser>()
                .Map(d => d.Email, s => s.Email)
                .Map(d => d.UserName, s => s.UserName)
                .Map(d => d.Id, s => Guid.NewGuid().ToString())
                .IgnoreNonMapped(true);

            config.NewConfig<AppUser, UserClaimsDTO>()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.UserName, s => s.UserName)
                .IgnoreNonMapped(true);

        }
    }
}
