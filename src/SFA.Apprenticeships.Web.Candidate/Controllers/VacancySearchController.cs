namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;

    public class VacancySearchController : Controller
    {
        // GET: VacancySearch
        public ActionResult Index()
        {
            return View();
        }

        // GET: VacancySearch/Results
        public ActionResult Results()
        {
            return View();
        }
    }
}