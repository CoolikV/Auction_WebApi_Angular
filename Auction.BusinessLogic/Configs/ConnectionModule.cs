using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Auction.DataAccess.Interfaces;
using Auction.DataAccess.Repositories;

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
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionStr);
        }
    }
}
