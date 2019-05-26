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
            routes.MapRoute("Firmalar", "firmalar",
               new { controller = "Firmalar", action = "Index" }
               );
            routes.MapRoute("YeniFirma", "yeni-firma",
               new { controller = "Firmalar", action = "NewCompany" }
               );
            routes.MapRoute("Income", "income",
               new { controller = "Monetary", action = "IncomeIndex" }
               );
            routes.MapRoute("Outgoing", "outgoing",
               new { controller = "Monetary", action = "OutgoingIndex" }
               );
            routes.MapRoute("CreateNewIncome", "new-income",
               new { controller = "Monetary", action = "NewIncome" }
               );
            routes.MapRoute("Login", "login", 
                new { controller = "Auth", action = "Login" }
                );
            routes.MapRoute("Logout", "logout", 
                new { controller = "Auth", action = "Logout" }
                );
            routes.MapRoute("Register", "register",
                new { controller = "Auth", action = "Register" }
                );
            routes.MapRoute("CompanyView", "companyView",
                new { controller = "Firmalar", action = "CompanyView" }
                );
            routes.MapRoute("CompanyDelete", "delete-company",
                new { controller = "Firmalar", action = "DeleteCompany" }
                );


        }
    }
}
