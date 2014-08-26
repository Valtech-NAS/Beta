namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;
    using Constants;
    using Controllers;
    
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as IUserContoller;
            if (controller == null) { throw new ConfigurationErrorsException("Controller must inherit from IUserController"); }
            
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (!controllerName.Equals("Login", StringComparison.InvariantCultureIgnoreCase))
            {
                controller.UserData.Pop(UserDataItemNames.SessionReturnUrl);
                var userContext = controller.UserData.GetUserContext();

                if (userContext != null)
                {
                    // They are logged in, set the timeout.
                    AddMetaRefreshTimeout(filterContext);
                }
            }

            controller.AuthenticationTicketService.RefreshTicket(filterContext.Controller.ControllerContext.HttpContext.Response.Cookies);
            base.OnActionExecuted(filterContext);
        }

        private void AddMetaRefreshTimeout(ActionExecutedContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var returnUrl = request != null && request.Url != null ? request.Url.PathAndQuery : "/";
            var helper = new UrlHelper(filterContext.RequestContext);
            var sessionTimeoutUrl = helper.Action("SessionTimeout", "Login", new { ReturnUrl = returnUrl });
            filterContext.RequestContext.HttpContext.Response.AppendHeader("Refresh", FormsAuthentication.Timeout.TotalSeconds + ";url=" + sessionTimeoutUrl);
        }
    }
}
