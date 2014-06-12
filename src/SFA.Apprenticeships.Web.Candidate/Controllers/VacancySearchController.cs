namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Framework;

    public class VacancySearchController : Controller
    {
        private readonly ISearchProvider _searchProvider;

        // TODO::needs to be config item?
        // Note::there is also a client side setting that limits the list on client side
        // This value limits data set to the client.
        public const int SearchPageSize = 10;
        private const int LocationResultCount = 25;

        public VacancySearchController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public ActionResult Index()
        {
            PopulateDistances();
            return View();
        }

        public ActionResult Search(VacancySearchResponseViewModel searchViewModel)
        {
            return Results(searchViewModel.VacancySearch);
        }

        public ActionResult Results(VacancySearchViewModel searchViewModel)
        {
            PopulateDistances(searchViewModel.WithinDistance);

            var location = new LocationViewModel(searchViewModel); 
            if (!searchViewModel.Latitude.HasValue || !searchViewModel.Longitude.HasValue)
            {
                // TODO::If no lat/long need to fail validation - this route is just for testing
                var locations = _searchProvider.FindLocation(searchViewModel.Location);
                location = locations.FirstOrDefault();
            }

            if (location != null )
            {
                var results = _searchProvider.FindVacancies(searchViewModel, location, SearchPageSize);
                if (!this.Request.IsAjaxRequest())
                {
                    return View("Results", results);
                }

                var view = this.ControllerContext.RenderPartialToString("_searchResults", results);
                return Json(view, JsonRequestBehavior.AllowGet);
            }

            // TODO::Test code, to be removed
            var vacancySearchResponseViewModel = new VacancySearchResponseViewModel
            {
                Vacancies = new List<VacancySummaryResponse>(),
                VacancySearch = searchViewModel
            };

            return View("Results", vacancySearchResponseViewModel);
        }

        public ActionResult Location(string term)
        {
            var matches = _searchProvider.FindLocation(term);

            if (this.Request.IsAjaxRequest())
            {
                return Json(matches.Take(LocationResultCount), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }

        public ActionResult Previous(VacancySearchViewModel searchViewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Next(VacancySearchViewModel searchViewModel)
        {
            throw new NotImplementedException();
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