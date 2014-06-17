namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.SessionState;
    using Infrastructure.Azure.Session;
    using NLog;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private readonly ISessionState _session;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public HomeController(ISessionState session)
        {
            _session = session;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            // TODO: Remove and add to admin page and add SpecFlow test to ensure Session working and configured correctly.
            int counter = 0;
            if (Session["counter"] != null)
            {
                counter = (int)Session["counter"];
            }

            Session.Add("Test", string.Format("This value is in session stored in Azure Cache with a counter: {0}", ++counter));
            Session["counter"] = counter;

            _logger.Info("Krister test");

            return View();
        }
	}
}