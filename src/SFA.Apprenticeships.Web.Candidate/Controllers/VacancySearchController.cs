namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

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
        public ActionResult Results(VacancySearchViewModel searchViewModel)
        {
            var locations = _searchProvider.FindLocation(searchViewModel.Location);

            if (locations.Count() == 1)
            {
                var results = _searchProvider.FindVacacnies(locations.First(), searchViewModel.WithinDistance);
                return View(results);    
            }

            return View();
        }
    }
}