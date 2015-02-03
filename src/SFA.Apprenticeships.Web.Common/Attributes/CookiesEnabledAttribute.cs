namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Web.Mvc;
    using Providers;

    public class CookiesEnabledAttribute : ActionFilterAttribute
    {
        public ICookieDetectionProvider CookieDetectionProvider { get; set; }

        public IEuCookieDirectiveProvider EuCookieDirectiveProvider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext) &&
                !filterContext.ActionDescriptor.ActionName.Equals("Cookies",
                    StringComparison.CurrentCultureIgnoreCase) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home",
                    StringComparison.CurrentCultureIgnoreCase))
            {
                CookieDetectionProvider.SetCookie(filterContext.HttpContext);

                var request = filterContext.RequestContext.HttpContext.Request;
                var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";
                var helper = new UrlHelper(filterContext.RequestContext);
                var url = helper.Action("Cookies", "Home", new {ReturnUrl = returnUrl});

                filterContext.Result = new RedirectResult(url);

                return;
            }

            if (CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext))
            {
                if (filterContext.ActionDescriptor.ActionName.Equals("Cookies",
                    StringComparison.CurrentCultureIgnoreCase)
                    &&
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home",
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    var url = filterContext.HttpContext.Request.QueryString.Get("returnUrl");

                    if (!string.IsNullOrEmpty(url))
                    {
                        filterContext.Result =
                            new RedirectResult(url);
                        return;
                    }
                }
                else
                {
                    filterContext.Controller.ViewBag.ShowEuCookieDirective =
                        EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}