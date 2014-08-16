namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;

    public class HomeController : CandidateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
