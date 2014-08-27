﻿namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Constants;
    using Providers;
    using StructureMap.Attributes;

    public class CookiesEnabledAttribute : ActionFilterAttribute
    {
        [SetterProperty]
        public ICookieDetectionProvider CookieDetectionProvider { get; set; }

        [SetterProperty]
        public IEuCookieDirectiveProvider EuCookieDirectiveProvider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext) &&
                !filterContext.ActionDescriptor.ActionName.Equals(RouteNames.Cookies,
                    StringComparison.CurrentCultureIgnoreCase))
            {
                CookieDetectionProvider.SetCookie(filterContext.HttpContext);

                filterContext.Result =
                    new RedirectResult(RouteNames.Cookies + "?returnUrl=" +
                                       HttpUtility.UrlEncode(filterContext.HttpContext.Request.Path));
                return;
            }

            if (CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext))
            {
                if (filterContext.ActionDescriptor.ActionName.Equals(RouteNames.Cookies,
                        StringComparison.CurrentCultureIgnoreCase))
                {
                    var originalUrlFromCookie = filterContext.HttpContext.Request.QueryString.Get("returnUrl");

                    if (!string.IsNullOrEmpty(originalUrlFromCookie))
                    {
                        filterContext.Result =
                            new RedirectResult(originalUrlFromCookie);
                        return;
                    }
                }
                else
                {
                    filterContext.Controller.ViewBag.ShowEuCookieDirective = !filterContext.ActionDescriptor.ActionName.Equals(RouteNames.Cookies,
                StringComparison.CurrentCultureIgnoreCase) && EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);

                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}