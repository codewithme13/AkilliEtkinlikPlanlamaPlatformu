using System.Web.Mvc;
using System.Web.Routing;

namespace Akıllı_Etkinlik_Planlama_Platformu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "EtkinlikKategori",
            url: "Etkinlikler/EtkinlikKategori",
            defaults: new { controller = "Etkinlikler", action = "EtkinlikKategori" }
        );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
