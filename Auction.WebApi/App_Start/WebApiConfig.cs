using Auction.BusinessLogic.Configs;
using Mapster;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;

namespace Auction.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //Mapster configuration
            TypeAdapterConfig.GlobalSettings.Scan(typeof(BLMapRegister).Assembly, Assembly.GetExecutingAssembly());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
