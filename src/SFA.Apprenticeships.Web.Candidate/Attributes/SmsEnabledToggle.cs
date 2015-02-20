namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Configuration;

    public class SmsEnabledToggle : ActionFilterAttribute
    {
        public IFeatureToggle FeatureToggle { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!FeatureToggle.IsActive(Feature.Sms))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}