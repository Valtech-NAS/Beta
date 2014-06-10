namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class VacancySearchController : Controller
    {
        private readonly ISearchProvider _searchProvider;

        // TODO::needs to be config item?
        private const int LocationResultCount = 25;

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
                var results = _searchProvider.FindVacancies(searchViewModel.JobTitle, 
                                                            searchViewModel.Keywords,
                                                            locations.First(), 
                                                            searchViewModel.PageNumber, 
                                                            searchViewModel.WithinDistance);
                results.VacancySearch = searchViewModel;
                return View(results);    
            }

            // Test code, to be removed
            var vacancySearchResponseViewModel = new VacancySearchResponseViewModel();
            vacancySearchResponseViewModel.Vacancies = new List<VacancySummaryResponse>();
            vacancySearchResponseViewModel.VacancySearch = searchViewModel;

            return View(vacancySearchResponseViewModel);
        }

        [HttpPost]
        public ActionResult Location(string term)
        {
            var matches = _searchProvider.FindLocation(term);

            if (this.Request.IsAjaxRequest())
            {
                return Json(matches.Take(LocationResultCount), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
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