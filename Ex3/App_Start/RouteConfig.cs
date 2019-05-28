using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute("display", "display/{ip}/{port}",
            defaults: new { controller = "Info", action = "display" });

            routes.MapRoute("save", "save/{ip}/{port}/{timesPerSec}/{seconds}/{fileName}",
            defaults: new { controller = "Info", action = "save" });
        
        
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Info", action = "info", id = UrlParameter.Optional });
                

    }
    
    }
}
