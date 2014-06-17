namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Azure.Session;

    public abstract class SfaControllerBase : Controller
    {
        protected SfaControllerBase(ISessionState session)
        {
            Session = session;
        }

        protected new ISessionState Session { get; private set; }
    }
}
