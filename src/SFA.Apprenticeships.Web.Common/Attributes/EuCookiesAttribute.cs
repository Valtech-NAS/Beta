namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Web.Mvc;
    using Constants;
    using Providers;

    public class EuCookiesAttribute : ActionFilterAttribute
    {
        private static bool _isCookieDetectionStarted;

        private ICookieDetectionProvider CookieDetectionProvider { get; set; }

        private IEuCookieDirectiveProvider EuCookieDirectiveProvider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ShowEuCookieDirective =
                EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);

            if (!CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext))
            {
                CookieDetectionProvider.SetCookie(filterContext.HttpContext);

                if (_isCookieDetectionStarted 
                    && !filterContext.ActionDescriptor.ActionName.Equals(RouteNames.Cookies, StringComparison.CurrentCultureIgnoreCase))
                {
                    filterContext.Result = new RedirectToRouteResult(RouteNames.Cookies, null);
                    return;
                }

                _isCookieDetectionStarted = true;
            }           


            base.OnActionExecuting(filterContext);
        }
    }
}
