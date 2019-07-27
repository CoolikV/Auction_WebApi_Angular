using Auction.DataAccess.EF;
using Auction.DataAccess.Interfaces;
using Auction.DataAccess.Repositories;
using Ninject.Modules;

namespace Auction.BusinessLogic.Configs
{
    public class ConnectionModule : NinjectModule
    {
        private readonly string connectionStr;

        public ConnectionModule(string connection)
        {
            connectionStr = connection;
        }

        public override void Load()
        {
            Bind<IDataContext>().To<AuctionContext>();
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionStr);
        }
    }
}
