namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using CuttingEdge.Conditions;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using Microsoft.Ajax.Utilities;
    using SFA.Apprenticeships.Web.Candidate.Providers;

    public class VacancySearchController : Controller
    {
        private ISearchProvider _searchProvider;

        public VacancySearchController()
        {
        }

        public VacancySearchController(ISearchProvider searchProvider)
        {
            Condition.Requires(searchProvider, "searchProvider").IsNotNull();

            _searchProvider = searchProvider;
        }

        // GET: VacancySearch
        public ActionResult Index()
        {
            return View();
        }

        // GET: VacancySearch/Results
        public ActionResult Results(VacancySearchViewModel searchViewModel)
        {
            return View();
        }
    }
}