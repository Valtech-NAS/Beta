namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using Constants;
    using Controllers;
    using NLog;

    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as IUserController;
            if (controller == null)
            {
                var message = string.Format("Controller {0} must inherit from IUserController",
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);

                Logger.Error(message);

                throw new ConfigurationErrorsException(message);
            }

            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (!controllerName.Equals("Login", StringComparison.InvariantCultureIgnoreCase))
            {
                controller.UserData.Pop(UserDataItemNames.SessionReturnUrl);
            }

            var httpContext = filterContext.Controller.ControllerContext.HttpContext;
            if (httpContext.User.Identity.IsAuthenticated)
            {
                // They are logged in, set the timeout.
                AddMetaRefreshTimeout(filterContext);

                // And refresh authentication ticket if required
                controller.AuthenticationTicketService.RefreshTicket(httpContext);
            }
            else
            {
                filterContext.Controller.ViewBag.EnableSessionTimeout = false;
            }

            base.OnActionExecuted(filterContext);
        }

        private void AddMetaRefreshTimeout(ActionExecutedContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";
            var helper = new UrlHelper(filterContext.RequestContext);
            var sessionTimeoutUrl = helper.Action("SessionTimeout", "Login", new { ReturnUrl = returnUrl });
            filterContext.Controller.ViewBag.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds;
            filterContext.Controller.ViewBag.SessionTimeoutUrl = sessionTimeoutUrl;
            filterContext.Controller.ViewBag.EnableSessionTimeout = true;
        }
    }
}
