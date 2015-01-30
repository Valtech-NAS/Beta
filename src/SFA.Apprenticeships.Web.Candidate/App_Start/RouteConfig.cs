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
                name: RouteNames.SignIn,
                url: "signin",
                defaults: new { controller = "Login", action = "Index" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Settings,
                url: "settings",
                defaults: new {controller = "Account", action = "Settings"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.MyApplications,
                url: "myapplications",
                defaults: new {controller = "Account", action = "Index"}
                );

            routes.MapRoute(
                name: RouteNames.UpdatedTermsAndConditions,
                url: "updatedtermsandconditions",
                defaults: new { controller = "Account", action = "UpdatedTermsAndConditions" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Privacy,
                url: "privacy",
                defaults: new {controller = "Home", action = "Privacy"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.Helpdesk,
                url: "helpdesk",
                defaults: new {controller = "Home", action = "Helpdesk"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.Terms,
                url: "terms",
                defaults: new {controller = "Home", action = "Terms"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipResults,
                url: "apprenticeships",
                defaults: new {controller = "ApprenticeshipSearch", action = "Results"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipSearch,
                url: "apprenticeshipsearch",
                defaults: new { controller = "ApprenticeshipSearch", action = "Index" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipSearchValidation,
                url: "apprenticeshipsearchvalidation",
                defaults: new { controller = "ApprenticeshipSearch", action = "SearchValidation" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipDetailsWithDistance,
                url: "apprenticeshipdetail/{id}/{distance}",
                defaults: new {controller = "ApprenticeshipSearch", action = "DetailsWithDistance"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipDetails,
                url: "apprenticeship/{id}",
                defaults: new {controller = "ApprenticeshipSearch", action = "Details"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipApply,
                url: "apprenticeship/apply/{id}",
                defaults: new {controller = "ApprenticeshipApplication", action = "Apply"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipPreview,
                url: "apprenticeship/preview/{id}",
                defaults: new {controller = "ApprenticeshipApplication", action = "Preview"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipWhatNext,
                url: "apprenticeship/whatnext/{id}",
                defaults: new {controller = "ApprenticeshipApplication", action = "WhatHappensNext"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipOverview,
                url: "traineeships/about",
                defaults: new {controller = "TraineeshipSearch", action = "Overview"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipSearch,
                url: "traineeshipsearch",
                defaults: new {controller = "TraineeshipSearch", action = "Index"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipApply,
                url: "traineeship/apply/{id}",
                defaults: new {controller = "TraineeshipApplication", action = "Apply"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipResults,
                url: "traineeships/search",
                defaults: new {controller = "TraineeshipSearch", action = "Results"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipDetails,
                url: "traineeship/{id}",
                defaults: new {controller = "TraineeshipSearch", action = "Details"}
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipWhatNext,
                url: "traineeship/whatnext/{id}",
                defaults: new {controller = "TraineeshipApplication", action = "WhatHappensNext"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: CandidateRouteNames.Maintenance,
                url: "maintenance",
                defaults: "~/403.aspx"
                );

            routes.LowercaseUrls = true;
        }
    }
}
