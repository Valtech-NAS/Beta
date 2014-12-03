namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;

    public class TraineeshipController : CandidateControllerBase
    {
        // GET: Traineeship
        public ActionResult Overview()
        {
            return View();
        }
    }
}