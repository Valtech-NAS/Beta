namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using Providers;
    using StructureMap.Attributes;

    public class PlannedOutageMessageAttribute : ActionFilterAttribute
    {
        private const string PlannedOutageMessageKey = "PlannedOutageMessage";

        [SetterProperty]
        public IConfigurationManager ConfigurationManager { get; set; }

        [SetterProperty]
        public IDismissPlannedOutageMessageCookieProvider DismissPlannedOutageMessageCookieProvider { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var plannedOutageMessage = ConfigurationManager.GetCloudAppSetting<string>(PlannedOutageMessageKey);
            if (!string.IsNullOrEmpty(plannedOutageMessage) && !DismissPlannedOutageMessageCookieProvider.IsCookiePresent(filterContext.HttpContext))
            {
                filterContext.Controller.ViewBag.PlannedOutageMessage = plannedOutageMessage;
            }
        }
    }
}