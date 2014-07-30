namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;
    using Domain.Entities.Applications;
    using Domain.Entities.Context;
    using Domain.Entities.Users;

    public interface IUserServiceProvider
    {
        UserContext GetUserContext(HttpContextBase httpContext);

        string[] GetUserClaims(HttpContextBase httpContext);

        void SetAuthenticationCookie(HttpContextBase httpContext, string userName, params string[] claims);

        void RefreshAuthenticationCookie(HttpContextBase httpContext);

        void DeleteAuthenticationCookie(HttpContextBase httpContext);

        void SetUserContextCookie(HttpContextBase httpContext, string userName, string fullName);

        void SetReturnUrlCookie(HttpContextBase httpContext, string returnUrl);

        string GetReturnUrl(HttpContextBase httpContext);

        void DeleteReturnUrlCookie(HttpContextBase httpContext);

        void DeleteAllCookies(HttpContextBase httpContext);

        void SetEntityContextCookie(HttpContextBase httpContext, Guid entityId, Guid viewModelId, string contextName);

        EntityContext GetEntityContextCookie(HttpContextBase httpContext, string contextName);
    }
}
