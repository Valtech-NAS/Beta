namespace SFA.Apprenticeships.Web.Common.Services
{
    using System;
    using System.Web;
    using System.Web.Security;

    public interface IAuthenticationTicketService
    {
        FormsAuthenticationTicket GetTicket(HttpCookieCollection cookies);

        void RefreshTicket(HttpContextBase httpContext);

        string[] GetClaims(FormsAuthenticationTicket ticket);

        void Clear(HttpCookieCollection cookies);

        void SetAuthenticationCookie(HttpCookieCollection cookies, string userName, params string[] claims);

        DateTime GetExpirationTimeFrom(FormsAuthenticationTicket ticket);
    }
}
