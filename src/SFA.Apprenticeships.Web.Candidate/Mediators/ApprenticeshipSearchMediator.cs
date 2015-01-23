namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchMediator : SearchMediatorBase, IApprenticeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly ApprenticeshipSearchViewModelServerValidator _searchRequestValidator;
        private readonly ApprenticeshipSearchViewModelLocationValidator _searchLocationValidator;

        public ApprenticeshipSearchMediator(
            IConfigurationManager configManager,
            ISearchProvider searchProvider,
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            IUserDataProvider userDataProvider,
            ApprenticeshipSearchViewModelServerValidator searchRequestValidator,
            ApprenticeshipSearchViewModelLocationValidator searchLocationValidator)
            : base(configManager, userDataProvider)
        {
            _searchProvider = searchProvider;
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> Index()
        {
            var distances = GetDistances();
            var sortTypes = GetSortTypes();
            var resultsPerPage = GetResultsPerPage();
            var apprenticeshipLevels = GetApprenticeshipLevels();
            var apprenticeshipLevel = GetApprenticeshipLevel();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                WithinDistance = 5,
                LocationType = ApprenticeshipLocationType.NonNational,
                Distances = distances,
                SortTypes = sortTypes,
                ResultsPerPage = resultsPerPage,
                ApprenticeshipLevels = apprenticeshipLevels,
                ApprenticeshipLevel = apprenticeshipLevel
            };

            return GetMediatorResponse(Codes.ApprenticeshipSearch.Index.Ok, viewModel);
        }

        private static SelectList GetApprenticeshipLevels(string selectedValue = "All")
        {
            var apprenticeshipLevels = new SelectList(
                new[]
                {
                    new {ApprenticeshipLevel = "All", Name = "All levels"},
                    new {ApprenticeshipLevel = "Intermediate", Name = "Intermediate"},
                    new {ApprenticeshipLevel = "Advanced", Name = "Advanced"},
                    new {ApprenticeshipLevel = "Higher", Name = "Higher"}
                },
                "ApprenticeshipLevel",
                "Name",
                selectedValue
                );

            return apprenticeshipLevels;
        }

        private string GetApprenticeshipLevel()
        {
            return UserDataProvider.Get(UserDataItemNames.ApprenticeshipLevel) ?? "All";
        }

        public MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(ApprenticeshipSearchViewModel model)
        {
            UserDataProvider.Pop(UserDataItemNames.VacancyDistance);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            UserDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

            if (string.IsNullOrEmpty(model.ApprenticeshipLevel))
            {
                model.ApprenticeshipLevel = GetApprenticeshipLevel();
            }

            UserDataProvider.Push(UserDataItemNames.ApprenticeshipLevel, model.ApprenticeshipLevel.ToString(CultureInfo.InvariantCulture));

            if (model.SearchAction == SearchAction.Search && model.LocationType != ApprenticeshipLocationType.NonNational)
            {
                model.LocationType = ApprenticeshipLocationType.NonNational;
            }

            if (model.LocationType == ApprenticeshipLocationType.NonNational && model.SortType == VacancySortType.Relevancy && string.IsNullOrWhiteSpace(model.Keywords))
            {
                model.SortType = VacancySortType.Distance;
            }

            if (model.LocationType == ApprenticeshipLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) && model.SortType != VacancySortType.ClosingDate)
            {
                model.SortType = VacancySortType.ClosingDate;
            }

            model.Distances = GetDistances(model.WithinDistance);
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);
            model.ApprenticeshipLevels = GetApprenticeshipLevels(model.ApprenticeshipLevel);

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.ValidationError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, clientResult);
            }

            if (!HasGeoPoint(model))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
                }

                if (suggestedLocations.Locations.Any())
                {
                    var location = suggestedLocations.Locations.First();

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
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Results.Ok, new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            var results = _searchProvider.FindVacancies(model);

            if (results.HasError())
            {
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

        public MediatorResponse<VacancyDetailViewModel> Details(int vacancyId, Guid? candidateId)
        {
            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(Codes.ApprenticeshipSearch.Details.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(Codes.ApprenticeshipSearch.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            var distance = UserDataProvider.Pop(UserDataItemNames.VacancyDistance);
            var lastVacancyId = UserDataProvider.Pop(UserDataItemNames.LastViewedVacancyId);

            if (HasToPopulateDistance(vacancyId, distance, lastVacancyId))
            {
                vacancyDetailViewModel.Distance = distance;
                UserDataProvider.Push(UserDataItemNames.VacancyDistance, distance);
            }

            UserDataProvider.Push(UserDataItemNames.LastViewedVacancyId, vacancyId.ToString(CultureInfo.InvariantCulture));

            return GetMediatorResponse(Codes.ApprenticeshipSearch.Details.Ok, vacancyDetailViewModel);
        }
    }
}