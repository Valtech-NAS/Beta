namespace SFA.Apprenticeships.Web.Common.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Security;

    public class AuthenticationTicketService : IAuthenticationTicketService
    {
        private static readonly string CookieName = FormsAuthentication.FormsCookieName;

        public FormsAuthenticationTicket GetTicket(HttpCookieCollection cookies)
        {
            try
            {
                if (!cookies.AllKeys.Contains(CookieName))
                {
                    return null;
                }

                var cookie = cookies[CookieName];

                if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
                {
                    return null;
                }

                return FormsAuthentication.Decrypt(cookie.Value);
            }
            catch (CryptographicException ex)
            {
                //Logger.Debug("Error decrypting ticket from cookie. Cookie is no longer valid and will be removed.", (Exception)ex);
                RemoveCookie(cookies);
                return null;
            }
            catch (Exception ex)
            {
                //Logger.Error("Error getting/decrypting ticket from cookie. Cookie is no longer valid and will be removed.", ex);
                RemoveCookie(cookies);
                return null;
            }
        }

        private static void RemoveCookie(HttpCookieCollection cookies)
        {
            try
            {
                cookies.Remove(CookieName);
            }
            catch (Exception ex)
            {
                //Logger.Error(string.Format("Error removing cookie {0}", CookieName), ex);
            }
        }

        public void RefreshTicket(HttpContextBase httpContext)
        {
            //Only extend the ticket if the cookie has not been set by a prior action or attribute
            if (httpContext.Response.Cookies.AllKeys.Contains(CookieName))
                return;

            var ticket = GetTicket(httpContext.Request.Cookies);

            if (ticket == null)
            {
                return;
            }

            var timeToExpiry = (ticket.Expiration - DateTime.Now).TotalSeconds;

            // Is the expiration within the update window?
            var expiring = timeToExpiry < (FormsAuthentication.Timeout.TotalSeconds / 2);

            if (!expiring)
            {
                return;
            }

            var newTicket = CreateTicket(ticket.Name, ArrayifyClaims(ticket));

            AddTicket(httpContext.Response.Cookies, newTicket);

            //Logger.Debug("Ticket issued for {0} because it only had {1}s to expire and the update window is {2}s", ticket.Name, timeToExpiry, (FormsAuthentication.Timeout.TotalSeconds / 2));
        }

        public string[] GetClaims(FormsAuthenticationTicket ticket)
        {
            return ArrayifyClaims(ticket);
        }

        public void Clear(HttpCookieCollection cookies)
        {
            if (!cookies.AllKeys.Contains(CookieName))
            {
                return;
            }

            cookies.Remove(CookieName);
        }

        public void SetAuthenticationCookie(HttpCookieCollection cookies, string userName, params string[] claims)
        {
            var ticket = CreateTicket(userName, claims);

            AddTicket(cookies, ticket);
        }

        #region Helpers
        private static void AddTicket(HttpCookieCollection cookies, FormsAuthenticationTicket ticket)
        {
            cookies.Add(new HttpCookie(CookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            });
        }

        private static FormsAuthenticationTicket CreateTicket(string userName, params string[] claims)
        {
            var expiration = DateTime.Now.AddSeconds(FormsAuthentication.Timeout.TotalSeconds);
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: userName,
                issueDate: DateTime.Now,
                expiration: expiration,
                isPersistent: false,
                userData: StringifyUserData(claims, expiration));

            //Logger.Debug("Ticket created for {0} with {1} at {2} expires {3}", ticket.Name, ticket.UserData, ticket.IssueDate, ticket.Expiration);

            return ticket;
        }

        private static string StringifyUserData(string[] claims, DateTime expiration)
        {
            var stringifiedClaims = string.Join(",", claims);
            return stringifiedClaims + ";" + expiration.ToString(CultureInfo.InvariantCulture);
        }

        private static string[] ArrayifyClaims(FormsAuthenticationTicket ticket)
        {
            if (ticket.UserData == null)
            {
                return new string[] { };
            }

            var claims = ticket.UserData.Split(new[] { ';' });

            return claims.First().Split(new[] { ',' });
        }

        public DateTime GetExpirationTimeFrom(FormsAuthenticationTicket ticket)
        {
            if (ticket.UserData == null)
            {
                return DateTime.MinValue;
            }

            var expirationTimeString = ticket.UserData.Split(new[] { ';' }).Last();
            // ReSharper disable once RedundantAssignment
            var expirationTime = DateTime.MinValue;
            DateTime.TryParse(expirationTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationTime);
            return expirationTime;
        }
        #endregion
    }
}
