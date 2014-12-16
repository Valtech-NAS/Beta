namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Common.Constants;
    using Common.Controllers;

    public class AuthorizeCandidateAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                OnUnauthorized(filterContext);
            }
        }

        private void OnUnauthorized(AuthorizationContext filterContext)
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
            else
            {
                var controller = filterContext.Controller as IUserController;
                if (controller != null)
                {
                    var userContext = controller.UserData.GetUserContext();
                    if (userContext != null)
                    {
                        //User was logged in but their authentication cookie has expired
                        OnUnauthorizedSessionExpired(filterContext);
                    }
                }
            }
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

        private void OnUnauthorizedSessionExpired(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary
            {
                {"Controller", "Login"},
                {"Action", "SessionTimeout"},
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