namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Web;
    using Domain.Entities.Users;

    public interface IUserServiceProvider
    {
        UserContext GetUserContext(HttpContextBase httpContext);

        string[] GetUserClaims(HttpContextBase httpContext);

        void SetAuthenticationCookie(HttpContextBase httpContext, string userName, params string[] claims);

        void RefreshAuthenticationCookie(HttpContextBase httpContext);

        void DeleteAuthenticationCookie(HttpContextBase httpContext);

        void SetUserContextCookie(HttpContextBase httpContext, string userName, string fullName);

        void SetAuthenticationReturnUrlCookie(HttpContextBase httpContext, string returnUrl);

        string GetAuthenticationReturnUrl(HttpContextBase httpContext);

        void DeleteAuthenticationReturnUrlCookie(HttpContextBase httpContext);
    }
}
