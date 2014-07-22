namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using System.Web.Routing;

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

            if (user.Identity.IsAuthenticated && !user.IsInRole("Activated"))
            {
                OnUnactivated(filterContext);
            }
        }

        private void OnUnactivated(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary
            {
                {"Controller", "Register"},
                {"Action", "Activation"},
                {"ReturnUrl", GetReturnUrl(filterContext)}
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

            return urlReferrer.ToString();
        }
    }
}