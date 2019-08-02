using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

namespace Auction.WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        private IUserManager UserManager { get; set; }
        public void ConfigureAuth(IAppBuilder app)
        {
            var userManager = GlobalConfiguration.Configuration.DependencyResolver
                            .GetService(typeof(IUserManager)) as IUserManager;

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new AppOAuthProvider(userManager),
                AuthorizeEndpointPath = new PathString("/api/account/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                AllowInsecureHttp = true,
                ApplicationCanDisplayErrors = true
            };

            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}