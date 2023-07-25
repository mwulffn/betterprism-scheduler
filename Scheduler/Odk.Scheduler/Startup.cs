using Owin;
using System.Web.Http;

namespace Odk.Scheduler
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //appBuilder.UseWebApi(config);
            appBuilder.UseNinject(() => Program.NinjectApiKernel).UseNinjectWebApi(config);
        }
    }
}