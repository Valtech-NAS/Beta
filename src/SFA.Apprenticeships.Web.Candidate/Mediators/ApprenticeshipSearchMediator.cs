namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Microsoft.Ajax.Utilities;
    using Providers;
    using Validators;
    using ViewModels;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchMediator : IApprenticeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly IUserDataProvider _userDataProvider;
        private readonly ApprenticeshipSearchViewModelClientValidator _searchRequestValidator;
        private readonly ApprenticeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly int _vacancyResultsPerPage;

        public ApprenticeshipSearchMediator(
            IConfigurationManager configManager,
            ISearchProvider searchProvider,
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            IUserDataProvider userDataProvider,
            ApprenticeshipSearchViewModelClientValidator searchRequestValidator,
            ApprenticeshipSearchViewModelLocationValidator searchLocationValidator)
        {
            _searchProvider = searchProvider;
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _userDataProvider = userDataProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;

            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> Index()
        {
            var distances = GetDistances();
            var sortTypes = GetSortTypes();
            var resultsPerPage = GetResultsPerPage();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                WithinDistance = 2,
                LocationType = ApprenticeshipLocationType.NonNational,
                Distances = distances,
                SortTypes = sortTypes,
                ResultsPerPage = resultsPerPage
            };

            return GetMediatorResponse(Codes.ApprenticeshipSearch.Index.Ok, viewModel);
        }

        public MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(ApprenticeshipSearchViewModel model, ModelStateDictionary modelState)
        {
            _userDataProvider.Pop(UserDataItemNames.VacancyDistance);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            _userDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

            if (model.SearchAction == SearchAction.Search && model.LocationType != ApprenticeshipLocationType.NonNational)
            {
                model.LocationType = ApprenticeshipLocationType.NonNational;
            }

            if (model.LocationType == ApprenticeshipLocationType.NonNational && model.SortType == VacancySortType.Relevancy && string.IsNullOrWhiteSpace(model.Keywords))
            {
                modelState.Remove("SortType");
                model.SortType = VacancySortType.Distance;
            }

            if (model.LocationType == ApprenticeshipLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) && model.SortType != VacancySortType.ClosingDate)
            {
                modelState.Remove("SortType");
                model.SortType = VacancySortType.ClosingDate;
            }

            model.Distances = GetDistances(model.WithinDistance);
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                modelState.Clear();
                clientResult.AddToModelState(modelState, string.Empty);

                return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.Ok, new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            if (!HasGeoPoint(model))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    modelState.Clear();
                    return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
                }

                if (suggestedLocations.Locations.Any())
                {
                    var location = suggestedLocations.Locations.First();

                    modelState.Remove("Location");
                    modelState.Remove("Latitude");
                    modelState.Remove("Longitude");

                    model.Location = location.Name;
                    model.Latitude = location.Latitude;
                    model.Longitude = location.Longitude;

                    model.LocationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
                    {
                        var vsvm = new ApprenticeshipSearchViewModel
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
                modelState.Clear();
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.Ok, new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            var results = _searchProvider.FindVacancies(model);

            if (results.HasError())
            {
                modelState.Clear();
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, results.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (model.SearchAction == SearchAction.Search && results.TotalLocalHits > 0)
            {
                results.VacancySearch.LocationType = ApprenticeshipLocationType.NonNational;
            }

            var isLocalLocationType = results.VacancySearch.LocationType != ApprenticeshipLocationType.National;
            results.VacancySearch.SortTypes = GetSortTypes(model.SortType, model.Keywords, isLocalLocationType);

            return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.Ok, results);
        }

        public MediatorResponse<VacancyDetailViewModel> Details(int id, Guid? candidateId, string searchReturnUrl)
        {
            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, id);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(Codes.ApprenticeshipSearch.Details.VacancyNotFound, null);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            var distance = _userDataProvider.Pop(UserDataItemNames.VacancyDistance);
            var lastVacancyId = _userDataProvider.Pop(UserDataItemNames.LastViewedVacancyId);

            if (HasToPopulateDistance(id, distance, lastVacancyId))
            {
                vacancyDetailViewModel.Distance = distance;
                _userDataProvider.Push(UserDataItemNames.VacancyDistance, distance);
            }

            vacancyDetailViewModel.SearchReturnUrl = searchReturnUrl;

            _userDataProvider.Push(UserDataItemNames.LastViewedVacancyId, id.ToStringInvariant());

            return GetMediatorResponse(Codes.ApprenticeshipSearch.Details.Ok, vacancyDetailViewModel);
        }
        private static SelectList GetDistances(int selectedValue = 2)
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

            return distances;
        }

        public SelectList GetSortTypes(VacancySortType selectedSortType = VacancySortType.Distance, string keywords = null, bool isLocalLocationType = true)
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

            var sortTypes = new SelectList(
                sortTypeOptions,
                "SortType",
                "Name",
                selectedSortType
                );

            return sortTypes;
        }

        private static SelectList GetResultsPerPageSelectList(int selectedValue)
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

            return resultsPerPage;
        }

        private int GetResultsPerPage()
        {
            int resultsPerPage;
            if (!int.TryParse(_userDataProvider.Get(UserDataItemNames.ResultsPerPage), out resultsPerPage))
            {
                resultsPerPage = _vacancyResultsPerPage;
            }

            return resultsPerPage;
        }

        private static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            searchViewModel.CheckLatLonLocHash();

            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        private static bool HasToPopulateDistance(int id, string distance, string lastVacancyId)
        {
            return !string.IsNullOrWhiteSpace(distance)
                   && !string.IsNullOrWhiteSpace(lastVacancyId)
                   && int.Parse(lastVacancyId) == id;
        }

        private static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel)
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel
            };

            return response;
        }

        private static MediatorResponse<T> GetMediatorResponse<T>(string code, T viewModel, string message, UserMessageLevel level) where T : ViewModelBase
        {
            var response = new MediatorResponse<T>
            {
                Code = code,
                ViewModel = viewModel,
                Message = new MediatorResponseMessage
                {
                    Message = message,
                    Level = level
                }
            };

            return response;
        }
    }
}