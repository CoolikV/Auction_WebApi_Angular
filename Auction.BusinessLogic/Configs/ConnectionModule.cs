using Auction.DataAccess.EF;
using Auction.DataAccess.Interfaces;
using Auction.DataAccess.Repositories;
using Ninject.Modules;

namespace Auction.BusinessLogic.Configs
{
    public class ConnectionModule : NinjectModule
    {
        public static string ConnectionString;

        public override void Load()
        {
            Bind<IDataContext>().To<AuctionContext>().InSingletonScope();
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(ConnectionString);
        }
    }
}
