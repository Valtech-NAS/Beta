using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SFA.Apprenticeships.Web.Employer
{
    using Constants;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico/{*pathInfo}");
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Content" });
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Scripts" });

            routes.MapRoute(
                name: EmployerRouteNames.SubmitEmployerEnquiry,
                url: "employerenquiry",
                defaults: new { controller = "EmployerEnquiry", action = "SubmitEmployerEnquiry" }
                );

            routes.MapRoute(
                name: EmployerRouteNames.ThankYou,
                url: "employerenquiry-thankyou",
                defaults: new { controller = "EmployerEnquiry", action = "ThankYou" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EmployerEnquiry", action = "SubmitEmployerEnquiry", id = UrlParameter.Optional }
                );
            routes.LowercaseUrls = true;
        }
    }
}
