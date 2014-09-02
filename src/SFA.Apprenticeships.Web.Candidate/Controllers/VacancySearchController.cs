namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using ActionResults;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Constants.Pages;
    using Constants.ViewModels;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Microsoft.Ajax.Utilities;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class VacancySearchController : CandidateControllerBase //todo: rename
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
            IVacancyDetailProvider vacancyDetailProvider)
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

            return View(new VacancySearchViewModel { WithinDistance = 2 });
        }

        [HttpGet]
        public ActionResult Results(VacancySearchViewModel model)
        {
            UserData.Pop(UserDataItemNames.VacancyDistance);

            if (searchViewModel.SortType == VacancySortType.Relevancy && string.IsNullOrWhiteSpace(searchViewModel.Keywords))
            {
                searchViewModel.SortType = VacancySortType.Distance;
                return RedirectToAction("results", searchViewModel);
            }

            if (!clientResult.IsValid)
            {
                ModelState.Clear();
                clientResult.AddToModelState(ModelState, string.Empty);

                return View("results", new VacancySearchResponseViewModel { VacancySearch = model });
            }

            model.CheckLatLonLocHash();

            if (!HasGeoPoint(model))
            {
                // Either user not selected item from dropdown or javascript disabled.
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
                            WithinDistance = model.WithinDistance
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
                return View("results", new VacancySearchResponseViewModel { VacancySearch = model });
            }

            var results = _searchProvider.FindVacancies(model, _vacancyResultsPerPage);

            if (results.HasError())
            {
                ModelState.Clear();
                SetUserMessage(results.ViewModelMessage, UserMessageLevel.Warning);

                return View("results", new VacancySearchResponseViewModel { VacancySearch = model });
            }

            PopulateDistances(searchViewModel.WithinDistance);
            PopulateSortType(searchViewModel.SortType, searchViewModel.Keywords);

            return View("results", results);
        }

        [HttpGet]
        public ActionResult DetailsWithDistance(int id, string distance)
        {
            UserData.Push(UserDataItemNames.VacancyDistance, distance.ToString(CultureInfo.InvariantCulture));
            UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToString(CultureInfo.InvariantCulture));

            return RedirectToAction("Details", new { id });
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Guid? candidateId = null;

            if (Request.IsAuthenticated)
            {
                candidateId = UserContext.CandidateId;
            }

            var vacancy = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, id);

            if (vacancy.HasError())
            {
                ModelState.Clear();
                SetUserMessage(vacancy.ViewModelMessage, UserMessageLevel.Warning);

                return View(vacancy);
            }

            if (vacancy == null)
            {
                return new VacancyNotFoundResult();
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
        }

        #region Helpers

        private static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        private void PopulateLookups(VacancySearchViewModel searchViewModel)
        {
            PopulateDistances(searchViewModel.WithinDistance);
            PopulateSortType(searchViewModel.SortType);
        }

        private VacancySearchViewModel[] FindSuggestedLocations(VacancySearchViewModel searchViewModel)
        {
            return null;
        }

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
                    new {WithinDistance = 40, Name = "40 miles"}
                },
                "WithinDistance",
                "Name",
                selectedValue
                );

            ViewBag.Distances = distances;
        }

        private void PopulateSortType(VacancySortType selectedSortType = VacancySortType.Distance, string keywords = null)
        {
            var sortTypeOptions = new ArrayList
            {
                new {SortType = VacancySortType.Distance, Name = "Distance"},
                new {SortType = VacancySortType.ClosingDate, Name = "Closing Date"}
            };

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                sortTypeOptions.Add(new {SortType = VacancySortType.Relevancy, Name = "Best Match"});
            }
            
            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
                );

            ViewBag.SortTypes = sortTypes;
        }

        #endregion
    }
}
