namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using Common.Controllers;
    using NLog;
    using System.Web.Mvc;
    using Infrastructure.Azure.Session;

    public class HomeController : SfaControllerBase
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public HomeController(ISessionState session) : base(session) {}

        public ActionResult Index()
        {
            _logger.Info("This is a test");
            return View();
        }
    }
}
