namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Constants;
    using Domain.Interfaces.Configuration;
    using StructureMap.Attributes;

    public class AuthorizeCandidateAttribute : AuthorizeAttribute
    {
        [SetterProperty]
        public IConfigurationManager ConfigurationManager { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            UserContext userContext = null;
            var controller = filterContext.Controller as IUserController;
            if (controller != null)
            {
                userContext = controller.UserData.GetUserContext();
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                OnUnauthorized(filterContext, userContext);
            }
            else if (userContext == null)
            {
                //Current session is invalid. Force user to log in again.
                OnSessionExpired(filterContext);
            }
            else if (
                !filterContext.RequestContext.HttpContext.Request.Path.ToLower().StartsWith("/updatedtermsandconditions") &&
                userContext.AcceptedTermsAndConditionsVersion != ConfigurationManager.GetAppSetting<string>(Settings.TermsAndConditionsVersion))
            {
                var routeValues = new RouteValueDictionary
                {
                    {"ReturnUrl", filterContext.RequestContext.HttpContext.Request.Path}
                };
                filterContext.Result = new RedirectToRouteResult(RouteNames.UpdatedTermsAndConditions, routeValues);
            }
        }

        private void OnUnauthorized(AuthorizationContext filterContext, UserContext userContext)
        {
            var user = filterContext.RequestContext.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole(UserRoleNames.Unactivated))
                {
                    OnUnauthorizedUnactivated(filterContext);
                    return;
                }

                OnUnauthorizedActivated(filterContext);
            }
            else if (userContext != null)
            {
                //User was logged in but their authentication cookie has expired
                OnSessionExpired(filterContext);
            }
        }

        private void OnSessionExpired(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary
            {
                {"Controller", "Login"},
                {"Action", "SessionTimeout"},
                {"ReturnUrl", GetReturnUrl(filterContext)}
            };

            filterContext.Result = new RedirectToRouteResult(routeValues);
        }

        private void OnUnauthorizedUnactivated(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary
            {
                {"Controller", "Register"},
                {"Action", "Activation"},
                {"ReturnUrl", GetReturnUrl(filterContext)}
            };

            filterContext.Result = new RedirectToRouteResult(routeValues);
        }

        private void OnUnauthorizedActivated(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary
            {
                {"Controller", "ApprenticeshipSearch"},
                {"Action", "Index"}
            };

            filterContext.Result = new RedirectToRouteResult(routeValues);
        }

        private static string GetReturnUrl(AuthorizationContext filterContext)
        {
            var urlReferrer = filterContext.RequestContext.HttpContext.Request.UrlReferrer;

            if (urlReferrer == null)
            {
                return string.Empty;
            }

            return urlReferrer.PathAndQuery;
        }
    }
}