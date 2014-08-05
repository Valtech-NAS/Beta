namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Linq;
    using System.Web;
    using Services;

    internal class UserServiceProvider : IUserServiceProvider
    {
        private static class CookieNames
        {
            public const string UserContext = "User.Context";
            public const string UserAuthReturnUrl = "User.Auth.ReturnUrl";
        }

        private static class CookieValueNames
        {
            public const string UserName = "UserName";
            public const string FullName = "FullName";
            public const string ViewModelId = "ViewModelId";
            public const string EntityId = "EntityId";
        }

        private readonly IAuthenticationTicketService _authenticationTicketService;

        public UserServiceProvider(IAuthenticationTicketService authenticationTicketService)
        {
            _authenticationTicketService = authenticationTicketService;
        }

        public UserContext GetUserContext(HttpContextBase httpContext)
        {
            if (!CookieExists(httpContext.Request.Cookies, CookieNames.UserContext))
            {
                return null;
            }

            var cookie = httpContext.Request.Cookies[CookieNames.UserContext];

            if (cookie == null)
            {
                return null;
            }

            return new UserContext
            {
                UserName = cookie.Values[CookieValueNames.UserName],
                FullName = cookie.Values[CookieValueNames.FullName]
            };
        }

        public string[] GetUserClaims(HttpContextBase httpContext)
        {
            var ticket = _authenticationTicketService.GetTicket(httpContext.Request.Cookies);

            return ticket == null ? new string[] { } : _authenticationTicketService.GetClaims(ticket);
        }

        public void SetAuthenticationCookie(HttpContextBase httpContext, string userName, params string[] claims)
        {
            var ticket = _authenticationTicketService.CreateTicket(userName, claims);

            _authenticationTicketService.AddTicket(httpContext.Response.Cookies, ticket);
        }

        public void RefreshAuthenticationCookie(HttpContextBase httpContext)
        {
            _authenticationTicketService.RefreshTicket(httpContext.Response.Cookies);
        }

        public void DeleteAuthenticationCookie(HttpContextBase httpContext)
        {
            _authenticationTicketService.DeleteTicket(httpContext.Response.Cookies);
        }

        public void SetUserContextCookie(HttpContextBase httpContext, string userName, string fullName)
        {
            var cookie = new HttpCookie(CookieNames.UserContext);

            cookie.Values.Add(CookieValueNames.UserName, userName);
            cookie.Values.Add(CookieValueNames.FullName, fullName);

            httpContext.Response.Cookies.Add(cookie);
        }

        public void SetReturnUrlCookie(HttpContextBase httpContext, string returnUrl)
        {
            var currentReturnUrl = GetReturnUrl(httpContext);

            if (!String.IsNullOrWhiteSpace(currentReturnUrl))
            {
                // Do not overwrite existing authentication return URL.
                return;
            }

            var cookie = new HttpCookie(CookieNames.UserAuthReturnUrl, returnUrl);

            httpContext.Response.Cookies.Add(cookie);
        }

        public string GetReturnUrl(HttpContextBase httpContext)
        {
            if (!CookieExists(httpContext.Request.Cookies, CookieNames.UserAuthReturnUrl))
            {
                return null;    
            }

            var cookie = httpContext.Request.Cookies[CookieNames.UserAuthReturnUrl];

            return cookie == null ? null : cookie.Value;
        }

        public void DeleteReturnUrlCookie(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies.Get(CookieNames.UserAuthReturnUrl) == null)
            {
                return;
            }

            httpContext.Response.Cookies.Add(CreateExpiredCookie(CookieNames.UserAuthReturnUrl));
        }

        public void DeleteAllCookies(HttpContextBase httpContext)
        {
            httpContext.Response.Cookies.Add(CreateExpiredCookie(CookieNames.UserContext));
            DeleteAuthenticationCookie(httpContext);
        }

        public void DeleteCookie(HttpContextBase httpContext, string cookieName)
        {
            httpContext.Response.Cookies.Add(CreateExpiredCookie(cookieName));
        }     

        public void SetEntityContextCookie(HttpContextBase httpContext, Guid entityId, Guid viewModelId, string contextName)
        {
            var cookie = new HttpCookie(contextName);

            cookie.Values.Add(CookieValueNames.ViewModelId, viewModelId.ToString());
            cookie.Values.Add(CookieValueNames.EntityId, entityId.ToString());
            httpContext.Response.Cookies.Add(cookie);
        }

        public EntityContext GetEntityContextCookie(HttpContextBase httpContext, string contextName)
        {
            if (!CookieExists(httpContext.Request.Cookies, contextName))
            {
                return null;
            }

            var cookie = httpContext.Request.Cookies[contextName];

            if (cookie == null)
            {
                return null;
            }

            return new EntityContext
            {
                EntityId = Guid.Parse(cookie.Values[CookieValueNames.EntityId]),
                ViewModelId = Guid.Parse(cookie.Values[CookieValueNames.ViewModelId])
            };
        }

        private static HttpCookie CreateExpiredCookie(string name)
        {
            var cookie = new HttpCookie(name)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            return cookie;
        }

        private static bool CookieExists(HttpCookieCollection cookies, string name)
        {
            return cookies.AllKeys.Contains(name);
        }
    }
}
