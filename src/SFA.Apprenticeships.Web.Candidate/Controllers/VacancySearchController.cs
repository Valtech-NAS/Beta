namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Search;
    using Common.Controllers;
    using Infrastructure.Azure.Session;
    using Infrastructure.Common.Configuration;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class VacancySearchController : SfaControllerBase
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IValidateModel<VacancySearchViewModel> _validator;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly int _vacancyResultsPerPage;
        private readonly int _locationResultLimit;

        public VacancySearchController(IConfigurationManager configManager, 
                                    ISearchProvider searchProvider, 
                                    IValidateModel<VacancySearchViewModel> validator,
                                    IVacancyDetailProvider vacancyDetailProvider,
                                    ISessionState session) : base (session)
        {
            _searchProvider = searchProvider;
            _validator = validator;
            _vacancyDetailProvider = vacancyDetailProvider;
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");
            _locationResultLimit = configManager.GetAppSetting<int>("LocationResultLimit");
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

            searchViewModel.CheckLatLonLocHash();
            var location = new LocationViewModel(searchViewModel);
            if (!searchViewModel.Latitude.HasValue || !searchViewModel.Longitude.HasValue)
            {
                //Either user not selected item from dropdown or javascript disabled.
                var locations = _searchProvider.FindLocation(searchViewModel.Location).ToList();
                if (locations.Any())
                {
                    location = locations.First();
                    searchViewModel.Location = location.Name;
                    searchViewModel.Latitude = location.Latitude;
                    searchViewModel.Longitude = location.Longitude;

                    if (locations.Count() > 1)
                    {
                        ViewBag.LocationSearches = locations.Skip(1).Select(l =>
                        {
                            var vsvm = new VacancySearchViewModel
                            {
                                Keywords = searchViewModel.Keywords,
                                Location = l.Name,
                                Latitude = l.Latitude,
                                Longitude = l.Longitude,
                                PageNumber = searchViewModel.PageNumber,
                                SortType = searchViewModel.SortType,
                                WithinDistance = searchViewModel.WithinDistance
                            };
                            vsvm.Hash = vsvm.LatLonLocHash();

                            return vsvm;
                        }).ToArray();
                    }
                }
            }

            if (!_validator.Validate(searchViewModel, ModelState))
            {
                return View("results", new VacancySearchResponseViewModel() {VacancySearch = searchViewModel});
            }

            if (location != null)
            {
                Session.Store("location", location);

                var results = _searchProvider.FindVacancies(searchViewModel, _vacancyResultsPerPage);
                return View("results", results);
            }

            return View("results", new VacancySearchResponseViewModel() { VacancySearch = searchViewModel });
        }

        [HttpGet]
        public ActionResult Location(string term)
        {
            var matches = _searchProvider.FindLocation(term);

            if (Request.IsAjaxRequest())
            {
                return Json(matches.Take(_locationResultLimit), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }

        [HttpGet]
        public ActionResult DetailsWithDistance(int id, string distance)
        {
            TempData["distance"] = distance;
            return RedirectToAction("Details", new {id});
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var vacancy = _vacancyDetailProvider.GetVacancyDetailViewModel(id);

            if (vacancy == null)
            {
                Response.StatusCode = 404;
                return View("vacancynotfound");
            }

            return View(vacancy);
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
                            new { WithinDistance = 0, Name = "Nationwide" }
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
                            new { SortType = VacancySortType.ClosingDate, Name = "Closing Date" }
                            //new { SortType = VacancySortType.Relevancy, Name = "Best Match" }
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