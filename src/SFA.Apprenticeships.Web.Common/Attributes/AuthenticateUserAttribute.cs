namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Security;
    using Services;

    // TODO: AG: review all references to "Debug.".

    public class AuthenticateUserAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            HttpContextBase httpContext = filterContext.RequestContext.HttpContext;

            var service = new AuthenticationTicketService();
            var ticket = service.GetTicket(httpContext.Request.Cookies);

            if (ticket == null)
            {
                // Debug.WriteLine("User is not logged in (no authentication ticket)");
                // Debug.WriteLine("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);
                return;
            }

            var claims = service.GetClaims(ticket);

            httpContext.User = new GenericPrincipal(new FormsIdentity(ticket), claims);

            // Debug.WriteLine("User.IsAuthenticated {0}", httpContext.User.Identity.IsAuthenticated);
            // Debug.WriteLine("Claims {0}", new object[] {string.Join(",", claims)});

            // Debug.WriteLine("Activated: {0}", httpContext.User.IsInRole("Activated"));
            // Debug.WriteLine("Unactivated: {0}", httpContext.User.IsInRole("Unactivated"));
            // Debug.WriteLine("Candidate: {0}", httpContext.User.IsInRole("Candidate"));
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}
