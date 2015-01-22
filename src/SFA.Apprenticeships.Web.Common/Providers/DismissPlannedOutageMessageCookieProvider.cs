namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class DismissPlannedOutageMessageCookieProvider : IDismissPlannedOutageMessageCookieProvider
    {
        private const string CookieName = "NAS.DismissPlannedOutageMessage";

        public void SetCookie(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieName);

            if (httpCookie != null)
            {
                return;
            }

            var cookie = new HttpCookie(CookieName)
            {
                Expires = DateTime.UtcNow.AddHours(18)
            };
            httpContext.Response.Cookies.Add(cookie);
        }

        public bool IsCookiePresent(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieName);
            return httpCookie != null;
        }
    }
}