namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Microsoft.Ajax.Utilities;
    using SFA.Apprenticeships.Web.Candidate.Providers;

    public class VacancySearchController : Controller
    {
        private ISearchProvider _searchProvider;

        public VacancySearchController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

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