namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        [OutputCache(CacheProfile = "Long")]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [OutputCache(CacheProfile = "Long")]
        public ActionResult InternalServerError()
        {
            return View("InternalServerError");
        }
    }
}
