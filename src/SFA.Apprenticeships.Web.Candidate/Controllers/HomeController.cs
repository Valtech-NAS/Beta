namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;

    public class HomeController : CandidateControllerBase
    {
        [OutputCache(CacheProfile = "Long")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "Long")]
        public ActionResult Privacy()
        {
            return View();
        }

        [OutputCache(CacheProfile = "Long")]
        public ActionResult Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [OutputCache(CacheProfile = "Long")]
        public ActionResult Helpdesk()
        {
            return View();
        }
    }
}