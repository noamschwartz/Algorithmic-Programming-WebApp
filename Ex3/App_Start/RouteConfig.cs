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
        /// <summary>
        /// this is the route different addresses given
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //address such as  /display/127.0.0.1/5400 and /display/flight1/4 
            routes.MapRoute("display", "display/{ip}/{port}",
            defaults: new { controller = "Info", action = "display" });
            //address such as /display/127.0.0.1/5400/4 
            routes.MapRoute("displayPerTime", "display/{ip}/{port}/{timesPerSec}",
            defaults: new { controller = "Info", action = "displayPerTime" });
            // address such as /save/127.0.0.1/5400/4/10/flight1 
            routes.MapRoute("save", "save/{ip}/{port}/{timesPerSec}/{secondsToShow}/{fileName}",
            defaults: new { controller = "Info", action = "save" });
            //default.
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Info", action = "info", id = UrlParameter.Optional });

    }
    
    }
}
