﻿using Auction.BusinessLogic.DataTransfer;
using Auction.WebApi.Models;
using Mapster;

namespace Auction.WebApi.App_Start
{
    public class WebMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.MaxDepth(3);

            config.NewConfig<TradingLotDTO, TradingLotModel>()
                .Map(dest => dest.Owner, src => src.User.UserName)
                .Map(d => d.Category, s => s.Category.Name);

            config.NewConfig<TradingLotModel, TradingLotDTO>()
                .Ignore(dest => dest.Category);

            config.NewConfig<RegisterModel, UserDTO>()
                .Map(dest => dest.Role, src => "user");
        }
    }
}