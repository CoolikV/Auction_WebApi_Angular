using Auction.BusinessLogic.Configs;
using Mapster;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

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

            config.EnableCors(new EnableCorsAttribute("http://localhost:4200", "*", "*"));
            //Mapster configuration
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.MaxDepth(3);
            TypeAdapterConfig.GlobalSettings.Scan(typeof(BLMapRegister).Assembly);

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
