namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using StructureMap.Attributes;

    public class ApplyWebTrendsAttribute : ActionFilterAttribute
    {
        [SetterProperty]
        public IConfigurationManager ConfigurationManager { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
            {
                return;
            }

            viewResult.ViewBag.EnableWebTrends = ConfigurationManager.GetAppSetting<bool>("EnableWebTrends");

            if (viewResult.ViewBag.EnableWebTrends == true)
            {
                viewResult.ViewBag.WebTrendsDscId = ConfigurationManager.GetAppSetting<string>("WebTrendsDscId");
                viewResult.ViewBag.WebTrendsDomainName = ConfigurationManager.GetAppSetting<string>("WebTrendsDomainName");            
            }

            base.OnActionExecuted(filterContext);
        }
    }
}