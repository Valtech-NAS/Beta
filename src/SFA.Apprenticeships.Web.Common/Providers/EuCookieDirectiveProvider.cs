namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class EuCookieDirectiveProvider : IEuCookieDirectiveProvider
    {
        private const string EuCookieName = "NAS.DisplayEuCookieDirective";

        public bool ShowEuCookieDirective(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(EuCookieName);

            if (httpCookie != null)
            {
                return false;
            }

            var cookie = new HttpCookie(EuCookieName)
            {
                Expires = DateTime.Now.AddDays(90)
            };

            httpContext.Response.Cookies.Add(cookie);

            return true;
        }
    }
}