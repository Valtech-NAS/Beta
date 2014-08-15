namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Constants;
    using Providers;

    [AuthenticateUser]
    public abstract class SfaControllerBase : Controller
    {
        protected SfaControllerBase(ISessionStateProvider session, IUserServiceProvider userServiceProvider)
        {
            Session = session;
            UserServiceProvider = userServiceProvider;
        }

        protected new ISessionStateProvider Session { get; private set; }

        protected IUserServiceProvider UserServiceProvider { get; private set; }

        protected UserContext UserContext { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserContext = UserServiceProvider.GetUserContext(filterContext.HttpContext);
            base.OnActionExecuting(filterContext);
        }

        protected void PushContextData(string key, string value)
        {
            //todo: refactor (don't use tempdata! push into context instead or use a non-session store?)
            TempData[key] = value;
        }

        protected string PopContextData(string key)
        {
            //todo: refactor (don't use tempdata! push into context instead or use a non-session store?)
            return TempData[key] as string;
        }

        protected void SetUserMessage(string message, UserMessageLevel level = UserMessageLevel.Success)
        {
            //todo: refactor (don't use tempdata! push into context instead or use a non-session store?)
            TempData[UserMessageConstants.InfoMessage] = message;
        }
    }
}
