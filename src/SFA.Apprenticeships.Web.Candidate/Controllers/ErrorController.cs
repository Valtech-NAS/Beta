namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Constants;

    public class ErrorController : Controller
    {
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public ActionResult InternalServerError()
        {
            return View("InternalServerError");
        }
    }
}
