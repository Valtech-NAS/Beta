namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Constants;

    public class HomeController : CandidateControllerBase
    {
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult Privacy()
        {
            return View();
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult Helpdesk()
        {
            return View();
        }
    }
}