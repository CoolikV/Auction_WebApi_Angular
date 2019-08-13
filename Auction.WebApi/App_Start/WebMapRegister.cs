using Auction.BusinessLogic.DataTransfer;
using Auction.WebApi.Models;
using Mapster;

namespace Auction.WebApi.App_Start
{
    public class WebMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.MaxDepth(3);

            config.NewConfig<TradingLotDTO, TradingLotModel>()
                .Map(dest => dest.Owner, src => src.User.UserName)
                .Map(d => d.Category, s => s.Category.Name)
                .Ignore(d => d.ImgBytes);

            config.NewConfig<TradingLotModel, TradingLotDTO>();

            config.NewConfig<BaseTradingLotModel, TradingLotDTO>()
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Img, s => s.Img)
                .Map(d => d.Price, s => s.Price)
                .Map(d => d.TradeDuration, s => s.TradeDuration)
                .Map(d => d.Description, s => s.Description)
                .Map(d => d.CategoryId, s => s.CategoryId)
                .IgnoreNonMapped(true);
            
            config.NewConfig<RegisterModel, UserDTO>()
                .Map(dest => dest.Role, src => "user");

            config.NewConfig<CategoryModel, CategoryDTO>().TwoWays();

            config.NewConfig<TradeDTO, TradeModel>()
                .Map(dest => dest.DaysLeft, src => src.TradeEnd.Subtract(src.TradeStart).Days);

            config.NewConfig<UserProfileModel, UserDTO>();

            config.NewConfig<UserDTO, UserProfileModel>()
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Surname, s => s.Surname)
                .Map(d => d.BirthDate, s => s.BirthDate)
                .Map(d => d.UserName, s => s.UserName)
                .IgnoreNonMapped(true);
        }
    }
}