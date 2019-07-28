using Auction.BusinessLogic.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Auction.WebApi.Startup))]
namespace Auction.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app, IUserManager userManager)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureAuth(app, userManager);

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}