namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Domain.Entities.Users;
    using Infrastructure.Azure.Session;

    [AuthenticateUser]
    public abstract class SfaControllerBase : Controller
    {
        protected SfaControllerBase(ISessionState session)
        {
            Session = session;
            
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // TODO: DONTKNOW: what if UserContext is null when used elsewhere?
            UserContext = GetUserContext();

            base.OnActionExecuting(filterContext);
        }

        protected new ISessionState Session { get; private set; }

        protected UserContext UserContext { get; private set; }

        private UserContext GetUserContext()
        {
            var cookie = Request.Cookies["User.Context"];

            if (cookie == null)
            {
                return null;
            }

            return new UserContext
            {
                UserName = cookie.Values["UserName"],
                FullName = cookie.Values["FullName"]
            };
        }
    }
}
