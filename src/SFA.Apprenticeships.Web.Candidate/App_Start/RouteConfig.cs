namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Common.Constants;
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
                name: RouteNames.SignOut,
                url: "signout",
                defaults: new {controller = "Login", action = "SignOut"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.Settings,
                url: "settings",
                defaults: new { controller = "Account", action = "Index" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.MyApplications,
                url: "myapplications",
                defaults: new { controller = "Application", action = "Index" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Privacy,
                url: "privacy",
                defaults: new { controller = "Home", action = "Privacy" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Helpdesk,
                url: "helpdesk",
                defaults: new { controller = "Home", action = "Helpdesk" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Terms,
                url: "terms",
                defaults: new { controller = "Home", action = "Terms" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Search,
                url: "apprenticeships",
                defaults: new { controller = "VacancySearch", action = "Results" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Results,
                url: "apprenticeshipsearch",
                defaults: new {controller = "VacancySearch", action = "Index"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.DetailsWithDistance,
                url: "apprenticeshipdetail/{id}/{distance}",
                defaults: new { controller = "VacancySearch", action = "DetailsWithDistance" }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Details,
                url: "apprenticeship/{id}",
                defaults: new { controller = "VacancySearch", action = "Details" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: CandidateRouteNames.Maintenance,
                url: "maintenance",
                defaults: "~/403.aspx");

            routes.LowercaseUrls = true;
        }
    }
}
