namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public interface IUserServiceProvider //todo: refactor! don't mention cookies!! remove unused stuff!!!
    {
        UserContext GetUserContext(HttpContextBase httpContext);

        string[] GetUserClaims(HttpContextBase httpContext);

        void SetAuthenticationCookie(HttpContextBase httpContext, string userName, params string[] claims);

        void RefreshAuthenticationCookie(HttpContextBase httpContext);

        void DeleteAuthenticationCookie(HttpContextBase httpContext);

        void SetUserContextCookie(HttpContextBase httpContext, string userName, string fullName);

        void DeleteAllCookies(HttpContextBase httpContext);

        void SetCookie(HttpContextBase httpContext, string cookieName, string value, bool allowOverwriteExisting = false);

        string GetCookie(HttpContextBase httpContext, string cookieName);

        void DeleteCookie(HttpContextBase httpContext, string cookieName);

        void SetEntityContextCookie(HttpContextBase httpContext, Guid entityId, Guid viewModelId, string contextName);

        EntityContext GetEntityContextCookie(HttpContextBase httpContext, string contextName);
    }
}
