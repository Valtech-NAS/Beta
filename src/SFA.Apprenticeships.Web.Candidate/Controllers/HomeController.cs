namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;

    public class HomeController : CandidateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Helpdesk()
        {
            return View();
        }
    }
}