namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Web.Mvc;

    public class AllowReturnUrlAttribute : ActionFilterAttribute
    {
        public AllowReturnUrlAttribute()
        {
            Order = Int32.MaxValue;
        }

        public bool Allow { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.AllowReturnUrl = Allow;
            base.OnActionExecuting(filterContext);
        }
    }
}