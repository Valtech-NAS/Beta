namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Web;

    public interface IEuCookieDirectiveProvider
    {         
        bool ShowEuCookieDirective(HttpContextBase httpContext);
    }
}