namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using System.Web.Security;
    using Constants;
    using NLog;
    using Services;

    public class AuthenticateUserAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var httpContext = filterContext.RequestContext.HttpContext;

            var service = new AuthenticationTicketService();
            var ticket = service.GetTicket(httpContext.Request.Cookies);

            if (ticket == null)
            {
                Logger.Debug("User is not logged in (no authentication ticket)");
                Logger.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);

                return;
            }

            if (IsCookieExpired(filterContext, service, ticket))
            {
                Logger.Debug("User cookie is expired.");

                filterContext.Result = new RedirectToRouteResult(RouteNames.SignOut, new RouteValueDictionary());
            }

            var claims = service.GetClaims(ticket);

            httpContext.User = new GenericPrincipal(new FormsIdentity(ticket), claims);

            Logger.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);
            Logger.Debug("Claims {0}", string.Join(",", claims));

            Logger.Debug("Activated: {0}", httpContext.User.IsInRole(UserRoleNames.Activated));
            Logger.Debug("Unactivated: {0}", httpContext.User.IsInRole(UserRoleNames.Unactivated));
        }

        private static bool IsCookieExpired(AuthenticationContext filterContext, AuthenticationTicketService service,
            FormsAuthenticationTicket ticket)
        {
            var expirationTime = service.GetExpirationTimeFrom(ticket);
            return (expirationTime < DateTime.Now && !SigningOut(filterContext));
        }

        private static bool SigningOut(AuthenticationContext filterContext)
        {
            return filterContext.ActionDescriptor.ActionName == "SignOut" &&
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Login";
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}
