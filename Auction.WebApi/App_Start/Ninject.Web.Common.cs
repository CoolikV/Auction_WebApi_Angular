[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Auction.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Auction.WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace Auction.WebApi.App_Start
{
    using Auction.BusinessLogic.Configs;
    using Auction.BusinessLogic.Interfaces;
    using Auction.BusinessLogic.Services;
    using Mapster;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using System;
    using System.Web;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new ConnectionModule("DefaultConnection"));
            kernel.Bind<IUserManager>().To<UserManager>();
            kernel.Bind<ICategoryService>().To<CategoryService>();
            kernel.Bind<ITradeService>().To<TradeService>();
            kernel.Bind<ITradingLotService>().To<TradingLotService>();

            kernel.Bind<IAdapter>().To<Adapter>().WithConstructorArgument(TypeAdapterConfig.GlobalSettings);
        }
    }
}