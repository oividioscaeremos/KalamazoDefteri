using System;
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
            routes.MapRoute("ViewUser", "view-user", 
                new { controller = "Home", action = "ViewUser" }
                );
            routes.MapRoute("Partial", "partial",
                new { controller = "Home", action = "PrintReport" }
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
            routes.MapRoute("DeleteIncome", "delete-income",
               new { controller = "Monetary", action = "DeleteIncome" }
               );
            routes.MapRoute("CreateNewOutgoing", "new-outgoing",
               new { controller = "Monetary", action = "NewOutgoing" }
               );
            routes.MapRoute("DeleteOutgoing", "delete-outgoing",
               new { controller = "Monetary", action = "DeleteOutgoing" }
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
            routes.MapRoute("PrintPartialViewToPdf", "print-report",
                new { controller = "Home", action = "PrintPartialViewToPdf" }
                );


        }
    }
}
