using Auction.DataAccess.EF;
using Auction.DataAccess.Interfaces;
using Auction.DataAccess.Repositories;
using Ninject.Modules;

namespace Auction.BusinessLogic.Configs
{
    public class ConnectionModule : NinjectModule
    {
        private readonly string _connectionString;

        public ConnectionModule(string connStr)
        {
            _connectionString = connStr;
        }
        public override void Load()
        {
            Bind<IDataContext>().To<AuctionContext>().InSingletonScope();
            Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope().WithConstructorArgument(_connectionString);
        }
    }
}
