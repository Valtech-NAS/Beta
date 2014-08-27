namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class CookieDetectionProvider : ICookieDetectionProvider
    {
        private const string CookieDetection = "NAS.CookieDetection";
        private const string CookieDetectionUrl = "NAS.OriginalUrl";

        public void SetCookie(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieDetection);

            if (httpCookie != null)
            {
                return;
            }

            var cookie = new HttpCookie(CookieDetection)
            {
                Expires = DateTime.Now.AddYears(5)
            };
            httpContext.Response.Cookies.Add(cookie);
        }

        public bool IsCookiePresent(HttpContextBase httpContext)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieDetection);
            return httpCookie != null;
        }

        public void SetOriginalUrlInCookie(HttpContextBase httpContextBase, string url)
        {
            var cookie = new HttpCookie(CookieDetectionUrl)
            {
                Value = url
            };
            httpContextBase.Response.Cookies.Add(cookie);
        }

        public string GetOriginalUrlFromCookie(HttpContextBase httpContextBase)
        {
            var httpCookie = httpContextBase.Request.Cookies.Get(CookieDetectionUrl);
            var url = httpCookie != null ? httpCookie.Value : string.Empty;

            httpCookie = new HttpCookie(CookieDetectionUrl)
            {
                Value =  string.Empty,
                Expires = DateTime.Now.AddDays(-1)
            };
            httpContextBase.Request.Cookies.Add(httpCookie);

            return url;
        }
    }
}