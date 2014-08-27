namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Web.Mvc;
    using Constants;
    using Providers;
    using StructureMap.Attributes;

    public class EuCookiesAttribute : ActionFilterAttribute
    {
        [SetterProperty]
        public IEuCookieDirectiveProvider EuCookieDirectiveProvider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ShowEuCookieDirective = !filterContext.ActionDescriptor.ActionName.Equals(RouteNames.Cookies,
                StringComparison.CurrentCultureIgnoreCase) && EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);

            base.OnActionExecuting(filterContext);
        }      
    }
}