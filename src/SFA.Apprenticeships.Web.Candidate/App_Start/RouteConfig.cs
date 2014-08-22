namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico/{*pathInfo}");
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Content" });
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Scripts" });

            routes.MapRoute(
                name: RouteNames.SignOut,
                url: "signout",
                defaults: new {controller = "Login", action = "SignOut"}
                );

            routes.MapRoute(
                name: RouteNames.Settings,
                url: "settings",
                defaults: new { controller = "Account", action = "Index" }
            );

            routes.MapRoute(
                name: RouteNames.MyApplications,
                url: "myapplications",
                defaults: new { controller = "Application", action = "Index" }
            );

            routes.MapRoute(
                name: RouteNames.Privacy,
                url: "privacy",
                defaults: new { controller = "Home", action = "Privacy" }
            );

            routes.MapRoute(
               name: RouteNames.Cookies,
               url: "cookies",
               defaults: new { controller = "Home", action = "Cookies" }
           );           

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.LowercaseUrls = true;
        }
    }
}
