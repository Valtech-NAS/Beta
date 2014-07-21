namespace SFA.Apprenticeships.Web.Common.Services
{
    using System.Web;
    using System.Web.Security;

    public interface IAuthenticationTicketService
    {
        FormsAuthenticationTicket CreateTicket(string userName, params string[] claims);
        void AddTicket(HttpCookieCollection cookies, FormsAuthenticationTicket ticket);
        FormsAuthenticationTicket GetTicket(HttpCookieCollection cookies);
        void DeleteTicket(HttpCookieCollection cookies);
        void RefreshTicket(HttpCookieCollection cookies);
        string[] GetClaims(FormsAuthenticationTicket ticket);
    }
}