namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchMediator : SearchMediatorBase, IApprenticeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IApprenticeshipVacancyDetailProvider _apprenticeshipVacancyDetailProvider;
        private readonly IReferenceDataService _referenceDataService;
        private readonly ApprenticeshipSearchViewModelServerValidator _searchRequestValidator;
        private readonly ApprenticeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly string[] _blacklistedCategoryCodes;

        public ApprenticeshipSearchMediator(
            IConfigurationManager configManager,
            ISearchProvider searchProvider,
            IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider,
            IUserDataProvider userDataProvider,
            IReferenceDataService referenceDataService,
            ApprenticeshipSearchViewModelServerValidator searchRequestValidator,
            ApprenticeshipSearchViewModelLocationValidator searchLocationValidator)
            : base(configManager, userDataProvider)
        {
            _searchProvider = searchProvider;
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            _referenceDataService = referenceDataService;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _blacklistedCategoryCodes = configManager.GetAppSetting("BlacklistedCategoryCodes").Split(',');
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> Index(ApprenticeshipSearchMode searchMode)
        {
            var distances = GetDistances();
            var sortTypes = GetSortTypes();
            var resultsPerPage = GetResultsPerPage();
            var apprenticeshipLevels = GetApprenticeshipLevels();
            var apprenticeshipLevel = GetApprenticeshipLevel();
            var categories = GetCategories();

            var viewModel = new ApprenticeshipSearchViewModel
            {
                WithinDistance = 5,
                LocationType = ApprenticeshipLocationType.NonNational,
                Distances = distances,
                SortTypes = sortTypes,
                ResultsPerPage = resultsPerPage,
                ApprenticeshipLevels = apprenticeshipLevels,
                ApprenticeshipLevel = apprenticeshipLevel,
                Categories = categories,
                SearchMode = searchMode
            };

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Index.Ok, viewModel);
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
            return UserDataProvider.Get(CandidateDataItemNames.ApprenticeshipLevel) ?? "All";
        }

        private List<Category> GetCategories()
        {
            var cats = _referenceDataService.GetCategories();
            return cats == null ? new List<Category>() : cats.Where(c => !_blacklistedCategoryCodes.Contains(c.CodeName)).ToList();
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> SearchValidation(ApprenticeshipSearchViewModel model)
        {
            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                model.Distances = GetDistances();
                model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);
                model.ApprenticeshipLevels = GetApprenticeshipLevels(model.ApprenticeshipLevel);
                model.Categories = GetCategories();

                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError, model, clientResult);
            }

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SearchValidation.Ok, model);
        }

        public MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(ApprenticeshipSearchViewModel model)
        {
            UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            UserDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

            if (string.IsNullOrEmpty(model.ApprenticeshipLevel))
            {
                model.ApprenticeshipLevel = GetApprenticeshipLevel();
            }

            UserDataProvider.Push(CandidateDataItemNames.ApprenticeshipLevel, model.ApprenticeshipLevel.ToString(CultureInfo.InvariantCulture));

            if (model.SearchAction == SearchAction.Search && model.LocationType != ApprenticeshipLocationType.NonNational)
            {
                model.LocationType = ApprenticeshipLocationType.NonNational;
            }

            PopulateSortType(model);
            
            model.Distances = GetDistances();
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);
            model.ApprenticeshipLevels = GetApprenticeshipLevels(model.ApprenticeshipLevel);
            model.Categories = GetCategories();

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.ValidationError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, clientResult);
            }

            if (!HasGeoPoint(model) && !string.IsNullOrEmpty(model.Location))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
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
                            ResultsPerPage = model.ResultsPerPage,
                            Category = model.Category,
                            SubCategories = model.SubCategories,
                            SearchMode = model.SearchMode
                        };

                        vsvm.Hash = vsvm.LatLonLocHash();

                        return vsvm;
                    }).ToArray();
                }
            }

            var locationResult = _searchLocationValidator.Validate(model);

            if (!locationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.Ok, new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            RemoveInvalidSubCategories(model);
            var searchModel = GetSearchModel(model);
            PopulateSortType(searchModel);
            model.SortType = searchModel.SortType;
            var results = _searchProvider.FindVacancies(searchModel);
            if (results.VacancySearch != null)
            {
                model.LocationType = results.VacancySearch.LocationType;
            }
            results.VacancySearch = model;

            if (results.HasError())
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, results.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (results.ExactMatchFound)
            {
                var id = results.Vacancies.Single().Id;
                return GetMediatorResponse<ApprenticeshipSearchResponseViewModel>(ApprenticeshipSearchMediatorCodes.Results.ExactMatchFound, parameters: new { id });
            }

            if (model.SearchAction == SearchAction.Search && results.TotalLocalHits > 0)
            {
                results.VacancySearch.LocationType = ApprenticeshipLocationType.NonNational;
            }

            var isLocalLocationType = results.VacancySearch.LocationType != ApprenticeshipLocationType.National;

            results.VacancySearch.SortTypes = GetSortTypes(model.SortType, model.Keywords, isLocalLocationType);

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.Ok, results);
        }

        private static ApprenticeshipSearchViewModel GetSearchModel(ApprenticeshipSearchViewModel model)
        {
            //Create a new search view model based on the search mode and user data
            var searchModel = new ApprenticeshipSearchViewModel(model);

            switch (searchModel.SearchMode)
            {
                case ApprenticeshipSearchMode.Keyword:
                    searchModel.Category = null;
                    searchModel.SubCategories = null;
                    break;
                case ApprenticeshipSearchMode.Category:
                    searchModel.Keywords = null;
                    searchModel.Categories = model.Categories;
                    break;
            }

            return searchModel;
        }

        private static void RemoveInvalidSubCategories(ApprenticeshipSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.Category) || model.SubCategories == null || model.Categories == null) return;
            var selectedCategory = model.Categories.SingleOrDefault(c => c.CodeName == model.Category);
            if (selectedCategory == null) return;
            model.SubCategories = model.SubCategories.Where(c => selectedCategory.SubCategories.Any(sc => sc.CodeName == c)).ToArray();
        }

        //TODO: Tell don't ask?
        private static void PopulateSortType(ApprenticeshipSearchViewModel model)
        {
            if (model.LocationType == ApprenticeshipLocationType.NonNational && model.SortType == VacancySearchSortType.Relevancy &&
                string.IsNullOrWhiteSpace(model.Keywords))
            {
                model.SortType = VacancySearchSortType.Distance;
            }

            if (model.LocationType == ApprenticeshipLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) &&
                model.SortType != VacancySearchSortType.ClosingDate)
            {
                model.SortType = VacancySearchSortType.ClosingDate;
            }

            if (model.SearchAction == SearchAction.Search && !string.IsNullOrWhiteSpace(model.Keywords))
            {
                model.SortType = VacancySearchSortType.Relevancy;
            }

            if (model.SearchAction == SearchAction.LocationTypeChanged)
            {
                if (!string.IsNullOrWhiteSpace(model.Keywords))
                {
                    model.SortType = VacancySearchSortType.Relevancy;
                }
                else if (model.LocationType == ApprenticeshipLocationType.National)
                {
                    model.SortType = VacancySearchSortType.ClosingDate;
                }
                else
                {
                    model.SortType = VacancySearchSortType.Distance;
                }
            }
        }

        public MediatorResponse<VacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<VacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            var vacancyDetailViewModel = _apprenticeshipVacancyDetailProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<VacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            if ((!vacancyDetailViewModel.CandidateApplicationStatus.HasValue && vacancyDetailViewModel.VacancyStatus != VacancyStatuses.Live) ||
                (vacancyDetailViewModel.CandidateApplicationStatus.HasValue && vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable))
            {
                // Candidate has no application for the vacancy and the vacancy is no longer live OR
                // candidate has an application (at least a draft) but the vacancy is no longer available.
                return GetMediatorResponse<VacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            var distance = UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);
            var lastVacancyId = UserDataProvider.Pop(CandidateDataItemNames.LastViewedVacancyId);

            if (HasToPopulateDistance(vacancyId, distance, lastVacancyId))
            {
                vacancyDetailViewModel.Distance = distance;
                UserDataProvider.Push(CandidateDataItemNames.VacancyDistance, distance);
            }

            UserDataProvider.Push(CandidateDataItemNames.LastViewedVacancyId, vacancyId.ToString(CultureInfo.InvariantCulture));

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Details.Ok, vacancyDetailViewModel);
        }
    }
}