namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Security;
    using NLog;
    using Services;

    public class AuthenticateUserAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            HttpContextBase httpContext = filterContext.RequestContext.HttpContext;

            var service = new AuthenticationTicketService();
            var ticket = service.GetTicket(httpContext.Request.Cookies);

            if (ticket == null)
            {
                Logger.Debug("User is not logged in (no authentication ticket)");
                Logger.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);

                return;
            }

            var claims = service.GetClaims(ticket);

            httpContext.User = new GenericPrincipal(new FormsIdentity(ticket), claims);

            Logger.Debug("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);
            Logger.Debug("Claims {0}", string.Join(",", claims));

            Logger.Debug("Activated: {0}", httpContext.User.IsInRole("Activated"));
            Logger.Debug("Unactivated: {0}", httpContext.User.IsInRole("Unactivated"));
            Logger.Debug("Candidate: {0}", httpContext.User.IsInRole("Candidate"));
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}
