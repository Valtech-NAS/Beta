namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Domain.Entities.Users;
    using Infrastructure.Azure.Session;
    using Providers;

    [AuthenticateUser]
    public abstract class SfaControllerBase : Controller
    {
        protected SfaControllerBase(ISessionState session, IUserServiceProvider userServiceProvider)
        {
            Session = session;
            UserServiceProvider = userServiceProvider;
        }

        protected new ISessionState Session { get; private set; }

        public IUserServiceProvider UserServiceProvider { get; private set; }

        protected UserContext UserContext { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserContext = UserServiceProvider.GetUserContext(filterContext.HttpContext);

            base.OnActionExecuting(filterContext);
        }
    }
}
