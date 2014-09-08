namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Constants;

    public class ErrorController : Controller
    {
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult InternalServerError()
        {
            return View("InternalServerError");
        }
    }
}
