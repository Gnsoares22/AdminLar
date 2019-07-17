using System.Web.Mvc;
using System.Web.Routing;

namespace AdminLar
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Landing", action = "Logar", id = UrlParameter.Optional }
            );
        }
    }
}
