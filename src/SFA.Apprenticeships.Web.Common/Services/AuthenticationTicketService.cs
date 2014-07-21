namespace SFA.Apprenticeships.Web.Common.Services
{
    using System;
    using System.Web;
    using System.Web.Security;
    using NLog;

    public class AuthenticationTicketService : IAuthenticationTicketService
    {
        private static readonly string CookieName = FormsAuthentication.FormsCookieName;

        private const int CookieUpdateWindow = 900;
        private const int CookieExpirationSeconds = 1800;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public FormsAuthenticationTicket CreateTicket(string userName, params string[] claims)
        {
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: userName,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddSeconds(CookieExpirationSeconds),
                isPersistent: false,
                userData: StringifyClaims(claims));

            Logger.Debug("Ticket created for {0} with {1} at {2} expires {3}",
                ticket.Name, ticket.UserData, ticket.IssueDate, ticket.Expiration);

            return ticket;
        }

        public void AddTicket(HttpCookieCollection cookies, FormsAuthenticationTicket ticket)
        {
            cookies.Add(new HttpCookie(CookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true
            });

            Logger.Debug("Ticket added: {0}", CookieName);
        }

        public FormsAuthenticationTicket GetTicket(HttpCookieCollection cookies)
        {
            var cookie = cookies[CookieName];

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                return null;
            }

            return FormsAuthentication.Decrypt(cookie.Value);
        }

        public void DeleteTicket(HttpCookieCollection cookies)
        {
            if (cookies[CookieName] == null)
            {
                return;
            }

            cookies.Add(CreateExpiredCookie());

            Logger.Debug("Ticket expired: {0}", CookieName);
        }

        public void RefreshTicket(HttpCookieCollection cookies)
        {
            var ticket = GetTicket(cookies);

            if (ticket == null)
            {
                return;
            }

            var timeToExpiry = (ticket.Expiration - DateTime.Now).TotalSeconds;

            // Is the expiration within the update window?
            var expiring = timeToExpiry < CookieUpdateWindow;

            if (expiring)
            {
                var newTicket = CreateTicket(ticket.Name, ArrayifyClaims(ticket));

                AddTicket(cookies, newTicket);

                Logger.Debug("Ticket issued for {0} because it only had {1}s to expire and the update window is {2}s",
                    ticket.Name, timeToExpiry, CookieUpdateWindow);
            }
        }

        public string[] GetClaims(FormsAuthenticationTicket ticket)
        {
            return ArrayifyClaims(ticket);
        }

        private static HttpCookie CreateExpiredCookie()
        {
            var cookie = new HttpCookie(CookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            return cookie;
        }

        private static string StringifyClaims(string[] claims)
        {
            return string.Join(",", claims);
        }

        private static string[] ArrayifyClaims(FormsAuthenticationTicket ticket)
        {
            if (ticket.UserData == null)
            {
                return new string[] { };
            }

            return ticket.UserData.Split(new[] { ',' });
        }
    }
}
