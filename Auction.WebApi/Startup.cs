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
        public void Configuration(IAppBuilder app)
        {
            //HttpConfiguration config = new HttpConfiguration();

            ConfigureAuth(app);

            app.UseWebApi(GlobalConfiguration.Configuration);
            app.UseCors(CorsOptions.AllowAll);
        }
    }
}