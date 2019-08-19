using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace Auction.WebApi.Providers
{
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserManager _userManager;

        public AppOAuthProvider(IUserManager userManager)
        {
            _userManager = userManager;
        }
        
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        [EnableCors("*","*","*")]
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            SetContextHeaders(context);
            System.Security.Claims.ClaimsIdentity claim = null;
            try
            {
                claim = await _userManager.Authenticate(context.UserName, context.Password);
            }
            catch (NotFoundException ex)
            {
                context.Response.Headers.Add("Authetication Error", new string[1] { ex.Message });
            }

            AuthenticationProperties properties = CreateProperties(context.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(claim, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(claim);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "UserName", userName }
            };
            return new AuthenticationProperties(data);
        }

        private void SetContextHeaders(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, PUT, DELETE, POST, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, Accept, Authorization" });
            context.Response.Headers.Add("Access-Control-Max-Age", new[] { "1728000" });
        }
    }
}