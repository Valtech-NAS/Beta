namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;

    public class TraineeshipSearchController : CandidateControllerBase
    {
        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult Search()
        {
            return RedirectToAction("Overview");
        }

        public ActionResult Details()
        {
            return RedirectToAction("Overview");
        }
    }
}
