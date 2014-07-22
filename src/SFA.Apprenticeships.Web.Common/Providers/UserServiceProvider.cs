namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;
    using Domain.Entities.Users;
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
        }

        private readonly IAuthenticationTicketService _authenticationTicketService;

        public UserServiceProvider(IAuthenticationTicketService authenticationTicketService)
        {
            _authenticationTicketService = authenticationTicketService;
        }

        public UserContext GetUserContext(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies.Get(CookieNames.UserContext);

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
            var ticket =
                _authenticationTicketService.GetTicket(httpContext.Request.Cookies);

            if (ticket == null)
            {
                return new string[] { };
            }

            return _authenticationTicketService.GetClaims(ticket);
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

        public void SetAuthenticationReturnUrlCookie(HttpContextBase httpContext, string returnUrl)
        {
            string currentReturnUrl = GetAuthenticationReturnUrl(httpContext);

            if (!String.IsNullOrWhiteSpace(currentReturnUrl))
            {
                // Do not overwrite existing authentication return URL.
                return;
            }

            var cookie = new HttpCookie(CookieNames.UserAuthReturnUrl, returnUrl);

            httpContext.Response.Cookies.Add(cookie);
        }

        public string GetAuthenticationReturnUrl(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies.Get(CookieNames.UserAuthReturnUrl);

            if (cookie == null)
            {
                return null;
            }

            return cookie.Value;
        }

        public void DeleteAuthenticationReturnUrlCookie(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies.Get(CookieNames.UserAuthReturnUrl) == null)
            {
                return;
            }

            httpContext.Response.Cookies.Add(CreateExpiredCookie(CookieNames.UserAuthReturnUrl));
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
