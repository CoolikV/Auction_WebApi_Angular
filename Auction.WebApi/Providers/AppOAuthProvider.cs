using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Auction.BusinessLogic.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Auction.WebApi.Providers
{
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserManager _userManager;

        public AppOAuthProvider(IUserManager userManager)
        {
            _userManager = userManager;
        }
        //responsible for validating the “Client”, 
        //in our case we have only one client so we’ll always return that its validated successfully.
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var claim = await _userManager.Authenticate(context.UserName, context.Password);

            AuthenticationProperties properties = CreateProperties(context.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(claim, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(claim);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}