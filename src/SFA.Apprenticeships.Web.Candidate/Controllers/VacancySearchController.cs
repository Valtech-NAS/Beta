namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
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
            PopulateDistances();
            return View();
        }

        // GET: VacancySearch/Results
        public ActionResult Results(VacancySearchViewModel searchViewModel)
        {
            PopulateDistances(searchViewModel.WithinDistance);
            var locations = _searchProvider.FindLocation(searchViewModel.Location);

            if (locations != null && locations.Count() == 1)
            {
                var results = _searchProvider.FindVacancies(locations.First(), searchViewModel.WithinDistance);
                results.VacancySearch = searchViewModel;
                return View(results);    
            }

            // Test code, to be removed
            var vacancySearchResponseViewModel = new VacancySearchResponseViewModel();
            vacancySearchResponseViewModel.Vacancies = new List<VacancySummary>();
            vacancySearchResponseViewModel.VacancySearch = searchViewModel;

            return View(vacancySearchResponseViewModel);
        }

        private void PopulateDistances(int selectedValue = 2)
        {
            var distances = new SelectList(
                new[] 
                        {
                            new { WithinDistance = 2, Name = "This area only" },
                            new { WithinDistance = 5, Name = "5 miles" },
                            new { WithinDistance = 10, Name = "10 miles" },
                            new { WithinDistance = 15, Name = "15 miles" },
                            new { WithinDistance = 20, Name = "20 miles" },
                            new { WithinDistance = 30, Name = "30 miles" },
                            new { WithinDistance = 40, Name = "40 miles" },
                            new { WithinDistance = 0, Name = "Nationwide" },
                        },
                "WithinDistance",
                "Name",
                selectedValue
            );
            ViewBag.Distances = distances;
        }
    }
}