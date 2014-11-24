namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using StructureMap.Attributes;

    public class SiteRootRedirect : ActionFilterAttribute
    {
        [SetterProperty]
        public IConfigurationManager ConfigurationManager { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var redirectUrl = ConfigurationManager.TryGetAppSetting("SiteRootRedirect");

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                filterContext.Result = new RedirectResult(redirectUrl);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}