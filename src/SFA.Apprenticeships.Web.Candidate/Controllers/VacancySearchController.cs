namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancy;
    using Providers;
    using ViewModels.VacancySearch;
    using Common.Framework;

    public class VacancySearchController : Controller
    {
        private readonly ISearchProvider _searchProvider;

        // TODO::needs to be config item?
        public const int SearchPageSize = 10;
        // Note::there is also a client side setting that limits the list on client side
        // This value limits data set to the client.
        private const int LocationResultCount = 25;

        public VacancySearchController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            PopulateDistances();
            return View(new VacancySearchViewModel { WithinDistance = 2 });
            PopulateSortType();
        }

        [HttpGet]
        public ActionResult Clear()
        {
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult Search(VacancySearchResponseViewModel searchViewModel)
        {
            return RedirectToAction("results", searchViewModel.VacancySearch);
        }

        [HttpGet]
        public ActionResult Results(VacancySearchViewModel searchViewModel)
        {
            PopulateDistances(searchViewModel.WithinDistance);
            PopulateSortType(searchViewModel.SortType);

            var location = new LocationViewModel(searchViewModel); 
            if (!searchViewModel.Latitude.HasValue || !searchViewModel.Longitude.HasValue)
            {
                // TODO::If no lat/long need to fail validation - this route is just for testing
                var locations = _searchProvider.FindLocation(searchViewModel.Location);
                location = locations.FirstOrDefault();
            }

            if (location != null )
            {
                var results = _searchProvider.FindVacancies(searchViewModel, SearchPageSize);
                if (!this.Request.IsAjaxRequest())
                {
                    return View("results", results);
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

            return View("results", vacancySearchResponseViewModel);
        }

        [HttpGet]
        public ActionResult Location(string term)
        {
            var matches = _searchProvider.FindLocation(term);

            if (this.Request.IsAjaxRequest())
            {
                return Json(matches.Take(LocationResultCount), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }

        #region Dropdown View Bag Helpers

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

        private void PopulateSortType(VacancySortType selectedSortType = VacancySortType.Distance)
        {
            var sortTypes = new SelectList(
                new[] 
                        {
                            new { SortType = VacancySortType.Distance, Name = "Distance" },
                            new { SortType = VacancySortType.ClosingDate, Name = "Closing Date" },
                            //new { SortType = VacancySortType.Relevancy, Name = "Relevancy (Test)" }
                        },
                "SortType",
                "Name",
                selectedSortType
            );

            ViewBag.SortTypes = sortTypes;
        }

        #endregion
    }
}