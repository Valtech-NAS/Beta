namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Application.Interfaces.Vacancies;
    using Attributes;
    using Common.Constants;
    using Constants;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Microsoft.Ajax.Utilities;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class VacancySearchController : CandidateControllerBase //todo: rename
    {
        private readonly VacancySearchViewModelLocationValidator _searchLocationValidator;
        private readonly ISearchProvider _searchProvider;
        private readonly VacancySearchViewModelClientValidator _searchRequestValidator;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly int _vacancyResultsPerPage;

        public VacancySearchController(IConfigurationManager configManager,
            ISearchProvider searchProvider,
            VacancySearchViewModelClientValidator searchRequestValidator,
            VacancySearchViewModelLocationValidator searchLocationValidator,
            IVacancyDetailProvider vacancyDetailProvider)
        {
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _vacancyDetailProvider = vacancyDetailProvider;
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                PopulateDistances();
                PopulateSortType();

                var resultsPerPage = GetResultsPerPage();

                return
                    View(new VacancySearchViewModel
                    {
                        WithinDistance = 2,
                        LocationType = VacancyLocationType.NonNational,
                        ResultsPerPage = resultsPerPage
                    });
            });
        }

        private int GetResultsPerPage()
        {
            int resultsPerPage;
            if (!int.TryParse(UserData.Get(UserDataItemNames.ResultsPerPage), out resultsPerPage))
            {
                resultsPerPage = _vacancyResultsPerPage;
            }


            return resultsPerPage;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Results(VacancySearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Pop(UserDataItemNames.VacancyDistance);

                if (model.ResultsPerPage == 0)
                {
                    model.ResultsPerPage = GetResultsPerPage();
                }

                UserData.Push(UserDataItemNames.ResultsPerPage,
                    model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

                if (model.SearchAction == SearchAction.Search && model.LocationType != VacancyLocationType.NonNational)
                {
                    model.LocationType = VacancyLocationType.NonNational;
                }

                if (model.LocationType == VacancyLocationType.NonNational && model.SortType == VacancySortType.Relevancy &&
                    string.IsNullOrWhiteSpace(model.Keywords))
                {
                    ModelState.Remove("SortType");
                    model.SortType = VacancySortType.Distance;
                }

                if (model.LocationType == VacancyLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) &&
                    model.SortType != VacancySortType.ClosingDate)
                {
                    ModelState.Remove("SortType");
                    model.SortType = VacancySortType.ClosingDate;
                }

                PopulateDistances(model.WithinDistance);
                PopulateResultsPerPage(model.ResultsPerPage);

                var clientResult = _searchRequestValidator.Validate(model);

                if (!clientResult.IsValid)
                {
                    ModelState.Clear();
                    clientResult.AddToModelState(ModelState, string.Empty);

                    return View("results", new VacancySearchResponseViewModel {VacancySearch = model});
                }

                if (!HasGeoPoint(model))
                {
                    // User did not select a location from the dropdown list, provide suggested locations.
                    var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                    if (suggestedLocations.HasError())
                    {
                        ModelState.Clear();
                        SetUserMessage(suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);

                        return View("results", new VacancySearchResponseViewModel { VacancySearch = model });
                    }

                    if (suggestedLocations.Locations.Any())
                    {
                        var location = suggestedLocations.Locations.First();

                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");

                        model.Location = location.Name;
                        model.Latitude = location.Latitude;
                        model.Longitude = location.Longitude;

                        ViewBag.LocationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
                        {
                            var vsvm = new VacancySearchViewModel
                            {
                                Keywords = model.Keywords,
                                Location = each.Name,
                                Latitude = each.Latitude,
                                Longitude = each.Longitude,
                                PageNumber = model.PageNumber,
                                SortType = model.SortType,
                                WithinDistance = model.WithinDistance,
                                ResultsPerPage = model.ResultsPerPage
                            };

                            vsvm.Hash = vsvm.LatLonLocHash();

                            return vsvm;
                        }).ToArray();
                    }
                }

                var locationResult = _searchLocationValidator.Validate(model);

                if (!locationResult.IsValid)
                {
                    ModelState.Clear();
                    return View("results", new VacancySearchResponseViewModel {VacancySearch = model});
                }

                var results = _searchProvider.FindVacancies(model);

                if (results.HasError())
                {
                    ModelState.Clear();
                    SetUserMessage(results.ViewModelMessage, UserMessageLevel.Warning);
                    return View("results", new VacancySearchResponseViewModel {VacancySearch = model});
                }

                if (model.SearchAction == SearchAction.Search && results.TotalLocalHits != 0)
                {
                    results.VacancySearch.LocationType = VacancyLocationType.NonNational;
                }

                if (results.VacancySearch.LocationType == VacancyLocationType.National)
                {
                    PopulateSortType(model.SortType, model.Keywords, false);
                }
                else
                {
                    PopulateSortType(model.SortType, model.Keywords);
                }

                return View("results", results);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public async Task<ActionResult> DetailsWithDistance(int id, string distance)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Push(UserDataItemNames.VacancyDistance, distance);
                UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToString(CultureInfo.InvariantCulture));

                return RedirectToAction("Details", new { id });
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Details(int id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                Guid? candidateId = null;

                if (Request.IsAuthenticated)
                {
                    candidateId = UserContext.CandidateId;
                }

                var vacancy = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, id);

                if (vacancy == null)
                {
                    return new VacancyNotFoundResult();
                }

                if (vacancy.HasError())
                {
                    ModelState.Clear();
                    SetUserMessage(vacancy.ViewModelMessage, UserMessageLevel.Warning);

                    return View(vacancy);
                }

                var distance = UserData.Pop(UserDataItemNames.VacancyDistance);
                var lastVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);

                if (!string.IsNullOrWhiteSpace(distance)
                    && !string.IsNullOrWhiteSpace(lastVacancyId)
                    && int.Parse(lastVacancyId) == id)
                {
                    ViewBag.Distance = distance;
                    UserData.Push(UserDataItemNames.VacancyDistance, distance);
                }

                UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToStringInvariant());

                return View(vacancy);
            });
        }

        #region Helpers

        private void PopulateResultsPerPage(int selectedValue)
        {
            var resultsPerPage = new SelectList(
                new[]
                {
                    new {ResultsPerPage = 5, Name = "5 per page"},
                    new {ResultsPerPage = 10, Name = "10 per page"},
                    new {ResultsPerPage = 25, Name = "25 per page"},
                    new {ResultsPerPage = 50, Name = "50 per page"}
                },
                "ResultsPerPage",
                "Name",
                selectedValue
                );

            ViewBag.ResultsPerPageSelectList = resultsPerPage;
        }

        private void PopulateDistances(int selectedValue = 2)
        {
            var distances = new SelectList(
                new[]
                {
                    new {WithinDistance = 2, Name = "2 miles"},
                    new {WithinDistance = 5, Name = "5 miles"},
                    new {WithinDistance = 10, Name = "10 miles"},
                    new {WithinDistance = 15, Name = "15 miles"},
                    new {WithinDistance = 20, Name = "20 miles"},
                    new {WithinDistance = 30, Name = "30 miles"},
                    new {WithinDistance = 40, Name = "40 miles"}
                },
                "WithinDistance",
                "Name",
                selectedValue
                );

            ViewBag.Distances = distances;
        }

        private void PopulateSortType(VacancySortType selectedSortType = VacancySortType.Distance,
            string keywords = null, bool isLocalLocationType = true)
        {
            var sortTypeOptions = new ArrayList();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sortTypeOptions.Add(new { SortType = VacancySortType.Relevancy, Name = "Best Match" });
            }

            sortTypeOptions.Add(new { SortType = VacancySortType.ClosingDate, Name = "Closing Date" });

            if (isLocalLocationType)
            {
                sortTypeOptions.Add(new { SortType = VacancySortType.Distance, Name = "Distance" });
            }

            ModelState.Remove("SortType");

            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
            );

            ViewBag.SortTypes = sortTypes;
        }

        private static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            searchViewModel.CheckLatLonLocHash();

            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        #endregion
    }
}