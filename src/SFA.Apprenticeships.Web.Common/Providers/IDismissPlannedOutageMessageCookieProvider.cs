namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Web;

    public interface IDismissPlannedOutageMessageCookieProvider
    {
        void SetCookie(HttpContextBase httpContext);

        bool IsCookiePresent(HttpContextBase httpContext);
    }
}