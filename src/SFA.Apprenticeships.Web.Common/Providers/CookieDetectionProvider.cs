namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class CookieDetectionProvider : ICookieDetectionProvider
    {
        private const string CookieDetection = "NAS.CookieDetection";

        public void SetCookie(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieDetection);

            if (httpCookie != null)
            {
                return;
            }

            var cookie = new HttpCookie(CookieDetection)
            {
                Expires = DateTime.UtcNow.AddYears(5)
            };
            httpContext.Response.Cookies.Add(cookie);
        }

        public bool IsCookiePresent(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieDetection);
            return httpCookie != null;
        }
    }
}