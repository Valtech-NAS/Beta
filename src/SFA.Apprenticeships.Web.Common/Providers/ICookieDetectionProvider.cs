namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Web;

    public interface ICookieDetectionProvider
    {
        void SetCookie(HttpContextBase httpContext);

        bool IsCookiePresent(HttpContextBase httpContext);
    }
}