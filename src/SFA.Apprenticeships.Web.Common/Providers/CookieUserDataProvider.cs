namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class CookieUserDataProvider : IUserDataProvider
    {
        private const string UserDataCookieName = "User.Data";
        private const string UserContextCookieName = "User.Context";
        private const string UserNameCookieName = "User.UserName";
        private const string FullNameCookieName = "User.FullName";

        private readonly HttpContextBase _httpContext;

        public CookieUserDataProvider(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public UserContext GetUserContext()
        {
            var cookie = _httpContext.Request.Cookies.Get(UserContextCookieName);

            return cookie == null
                ? null
                : new UserContext
                {
                    UserName = cookie.Values[UserNameCookieName],
                    FullName = cookie.Values[FullNameCookieName]
                };
        }

        public void SetUserContext(string userName, string fullName)
        {
            var cookie = new HttpCookie(UserContextCookieName);

            cookie.Values.Add(UserNameCookieName, userName);
            cookie.Values.Add(FullNameCookieName, fullName);

            _httpContext.Response.Cookies.Add(cookie);
        }

        public void Clear()
        {
            _httpContext.Response.Cookies.Add(CreateExpiredCookie(UserContextCookieName));
            _httpContext.Response.Cookies.Add(CreateExpiredCookie(UserDataCookieName));
        }

        public void Push(string key, string value)
        {
            var dataCookies = GetOrCreateDataCookie(_httpContext.Response.Cookies);

            dataCookies.Values.Add(key, value);
        }

        public string Get(string key)
        {
            var dataCookies = GetOrCreateDataCookie(_httpContext.Request.Cookies);

            if (dataCookies.Values[key] == null)
            {
                return null;
            }

            return dataCookies.Values[key];
        }

        public string Pop(string key)
        {
            var value = Get(key);

            var dataCookies = GetOrCreateDataCookie(_httpContext.Response.Cookies);

            dataCookies.Values.Remove(key);

            return value;
        }

        private static HttpCookie GetOrCreateDataCookie(HttpCookieCollection cookies)
        {
            var dataCookie = cookies.Get(UserDataCookieName);

            if (dataCookie == null)
            {
                dataCookie = new HttpCookie(UserDataCookieName);
                cookies.Add(dataCookie);
            }

            return dataCookie;
        }

        private static HttpCookie CreateExpiredCookie(string name)
        {
            var cookie = new HttpCookie(name)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            return cookie;
        }
    }
}
