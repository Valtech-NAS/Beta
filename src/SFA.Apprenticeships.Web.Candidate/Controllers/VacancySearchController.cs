namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Search;
    using Common.Controllers;
    using Common.Framework;
    using Infrastructure.Azure.Session;
    using Infrastructure.Common.Configuration;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class VacancySearchController : SfaControllerBase
    {
        private readonly IConfigurationManager _configManager;
        private readonly ISearchProvider _searchProvider;
        private readonly IValidateModel<VacancySearchViewModel> _validator;

        public VacancySearchController(IConfigurationManager configManager, 
                                    ISearchProvider searchProvider, 
                                    IValidateModel<VacancySearchViewModel> validator,
                                    ISessionState session) : base (session)
        {
            _configManager = configManager;
            _searchProvider = searchProvider;
            _validator = validator;
        }

        private int VacancyResultsPerPage
        {
            get { return _configManager.GetAppSetting<int>("VacancyResultsPerPage"); }
        }

        private int LocationResultLimit
        {
            get { return _configManager.GetAppSetting<int>("LocationResultLimit"); }
        }

        [HttpGet]
        public ActionResult Index()
        {
            PopulateDistances();
            PopulateSortType();
            return View(new VacancySearchViewModel { WithinDistance = 2 });
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
                //Either user not selected item from dropdown or javascript disabled.
                var locations = _searchProvider.FindLocation(searchViewModel.Location);
                if (locations.Count() == 1)
                {
                    location = locations.Single();
                    searchViewModel.Latitude = location.Latitude;
                    searchViewModel.Longitude = location.Longitude;
                }
            }

            if (!_validator.Validate(searchViewModel, ModelState))
            {
                return View("index", searchViewModel);
            }

            if (location != null )
            {
                Session.Store("location", location);
                var results = _searchProvider.FindVacancies(searchViewModel, VacancyResultsPerPage);
                if (!this.Request.IsAjaxRequest())
                {
                    return View("results", results);
                }

                var view = this.ControllerContext.RenderPartialToString("_searchResults", results);
                return Json(view, JsonRequestBehavior.AllowGet);
            }

            return View("index", searchViewModel);
        }

        [HttpGet]
        public ActionResult Location(string term)
        {
            var matches = _searchProvider.FindLocation(term);

            if (this.Request.IsAjaxRequest())
            {
                return Json(matches.Take(LocationResultLimit), JsonRequestBehavior.AllowGet);
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