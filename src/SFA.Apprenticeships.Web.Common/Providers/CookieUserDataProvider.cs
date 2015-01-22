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
        private const string AcceptedTermsAndConditionsVersion = "User.TermsConditionsVersion";

        private readonly HttpContextBase _httpContext;
        private readonly HttpCookie _httpDataCookie;

        public CookieUserDataProvider(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
            _httpDataCookie = GetOrCreateDataCookie();
        }

        public UserContext GetUserContext()
        {
            var cookie = _httpContext.Request.Cookies.Get(UserContextCookieName);

            return cookie == null
                ? null
                : new UserContext
                {
                    UserName = cookie.Values[UserNameCookieName],
                    FullName = cookie.Values[FullNameCookieName],
                    AcceptedTermsAndConditionsVersion = cookie.Values[AcceptedTermsAndConditionsVersion]
                };
        }

        public void SetUserContext(string userName, string fullName, string acceptedTermsAndConditionsVersion)
        {
            var cookie = new HttpCookie(UserContextCookieName);

            cookie.Values.Add(UserNameCookieName, userName);
            cookie.Values.Add(FullNameCookieName, fullName);
            cookie.Values.Add(AcceptedTermsAndConditionsVersion, acceptedTermsAndConditionsVersion);

            _httpContext.Response.Cookies.Add(cookie);
        }

        public void Clear()
        {
            _httpContext.Response.Cookies.Add(CreateExpiredCookie(UserContextCookieName));
            _httpDataCookie.Values.Clear();
        }

        public void Push(string key, string value)
        {
            _httpDataCookie.Values.Remove(key);
            _httpDataCookie.Values.Add(key, _httpContext.Server.UrlEncode(value));
        }

        public string Get(string key)
        {
            if (_httpDataCookie == null || _httpDataCookie.Values[key] == null)
            {
                return null;
            }

            return _httpContext.Server.UrlDecode(_httpDataCookie.Values[key]);
        }

        public string Pop(string key)
        {
            var value = Get(key);

            if (value == null)
            {
                return null;
            }

            _httpDataCookie.Values.Remove(key);

            return value;
        }

        private HttpCookie GetOrCreateDataCookie()
        {
            var requestDataCookie = _httpContext.Request.Cookies.Get(UserDataCookieName);
            var responseDataCookie = _httpContext.Response.Cookies.Get(UserDataCookieName);

            var dataCookie = new HttpCookie(UserDataCookieName);

            if (requestDataCookie != null && requestDataCookie.Values.HasKeys() 
                && (responseDataCookie == null || !responseDataCookie.Values.HasKeys()))
            {
                foreach (var key in requestDataCookie.Values.AllKeys)
                {
                    dataCookie.Values.Add(key, requestDataCookie.Values[key]);
                }
            }

            if (responseDataCookie == null)
            {
                _httpContext.Response.Cookies.Add(dataCookie);
            }
            else
            {
                dataCookie = responseDataCookie.Values.HasKeys() ? responseDataCookie : dataCookie;
                _httpContext.Response.Cookies.Set(dataCookie);
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
