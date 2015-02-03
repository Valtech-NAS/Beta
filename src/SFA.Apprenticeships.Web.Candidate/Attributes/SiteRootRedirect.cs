namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;

    public class SiteRootRedirect : ActionFilterAttribute
    {
        public IConfigurationManager ConfigurationManager { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var redirectUrl = ConfigurationManager.TryGetAppSetting("SiteRootRedirectUrl");

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                filterContext.Result = new RedirectResult(redirectUrl);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}