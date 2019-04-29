﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KalamazoDefteri
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            

            routes.MapRoute("Home", "", 
                new { controller = "Home", action = "Index" }
                );

            routes.MapRoute("Login", "login", 
                new { controller = "Auth", action = "Login" }
                );


        }
    }
}
