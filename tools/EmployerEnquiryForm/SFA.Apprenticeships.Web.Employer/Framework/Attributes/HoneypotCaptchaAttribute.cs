namespace SFA.Apprenticeships.Web.Employer.Framework.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class HoneypotCaptchaAttribute : ActionFilterAttribute
    {
        public HoneypotCaptchaAttribute(string formField)
        {
            if (string.IsNullOrWhiteSpace(formField)) throw new ArgumentNullException("formField");
            FormField = formField;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "POST")
            {
                var value = filterContext.HttpContext.Request.Form[FormField];

                if (value == null || value.Length > 0)
                {
                    throw new HttpException(403, "Stop spamming this site.");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public string FormField { get; set; }
    }
}