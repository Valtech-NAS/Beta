using SFA.Apprenticeships.Infrastructure.Common.Configuration;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    using Common.Controllers;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Infrastructure.Azure.Session;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class VacancySearchController : SfaControllerBase
    {
        private readonly ISearchProvider _searchProvider;
        private readonly VacancySearchViewModelClientValidator _searchRequestValidator;
        private readonly VacancySearchViewModelLocationValidator _searchLocationValidator;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly int _vacancyResultsPerPage;

        public VacancySearchController(IConfigurationManager configManager,
            ISearchProvider searchProvider,
            VacancySearchViewModelClientValidator searchRequestValidator,
            VacancySearchViewModelLocationValidator searchLocationValidator,
            IVacancyDetailProvider vacancyDetailProvider,
            ISessionState session) : base(session)
        {
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _vacancyDetailProvider = vacancyDetailProvider;
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");
        }

        [HttpGet]
        public ActionResult Index()
        {
            PopulateDistances();
            PopulateSortType();

            return View(new VacancySearchViewModel {WithinDistance = 2});
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

            var clientResult = _searchRequestValidator.Validate(searchViewModel);
            if (!clientResult.IsValid)
            {
                ModelState.Clear();
                clientResult.AddToModelState(ModelState, string.Empty);
                return View("results", new VacancySearchResponseViewModel { VacancySearch = searchViewModel });
            }

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

            var locationResult = _searchLocationValidator.Validate(searchViewModel);
            if (!locationResult.IsValid)
            {
                ModelState.Clear();
                locationResult.AddToModelState(ModelState, string.Empty);
                return View("results", new VacancySearchResponseViewModel { VacancySearch = searchViewModel });
            }

            var results = _searchProvider.FindVacancies(searchViewModel, _vacancyResultsPerPage);
            return View("results", results);
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
                return View("VacancyNotFound");
            }

            return View(vacancy);
        }

        #region Dropdown View Bag Helpers
        private void PopulateDistances(int selectedValue = 2)
        {
            var distances = new SelectList(
                new[]
                {
                    new {WithinDistance = 2, Name = "This area only"},
                    new {WithinDistance = 5, Name = "5 miles"},
                    new {WithinDistance = 10, Name = "10 miles"},
                    new {WithinDistance = 15, Name = "15 miles"},
                    new {WithinDistance = 20, Name = "20 miles"},
                    new {WithinDistance = 30, Name = "30 miles"},
                    new {WithinDistance = 40, Name = "40 miles"},
                    new {WithinDistance = 0, Name = "Nationwide"}
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
                    new {SortType = VacancySortType.Distance, Name = "Distance"},
                    new {SortType = VacancySortType.ClosingDate, Name = "Closing Date"}
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
