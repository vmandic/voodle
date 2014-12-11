using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Voodle.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // TODO: make'em happen!
            //routes.MapRoute("Error404", "Error/Error404", MVC.Error.Error404());
            //routes.MapRoute("Error500", "Error/Error500", MVC.Error.Error500());
        }
    }
}
