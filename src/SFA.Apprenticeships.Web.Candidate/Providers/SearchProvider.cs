namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Infrastructure.PerformanceCounters;
    using NLog;
    using ViewModels;
    using ViewModels.Locations;
    using ViewModels.VacancySearch;
    using ErrorCodes = Application.Interfaces.Locations.ErrorCodes;

    public class SearchProvider : ISearchProvider
    {
        private const string WebRolePerformanceCounterCategory = "SFA.Apprenticeships.Web.Candidate";
        private const string VacancySearchCounter = "VacancySearch";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAddressSearchService _addressSearchService;
        private readonly ILocationSearchService _locationSearchService;
        private readonly IMapper _apprenticeshipSearchMapper;
        private readonly IMapper _traineeshipSearchMapper;
        private readonly IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _apprenticeshipSearchService;
        private readonly IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters> _traineeshipSearchService;
        private readonly IPerformanceCounterService _performanceCounterService;
        private readonly IConfigurationManager _configurationManager;

        public SearchProvider(ILocationSearchService locationSearchService,
            IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> apprenticeshipSearchService,
            IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters> traineeshipSearchService,
            IAddressSearchService addressSearchService,
            IMapper apprenticeshipSearchMapper,
            IMapper traineeshipSearchMapper, 
            IPerformanceCounterService performanceCounterService, 
            IConfigurationManager configurationManager)
        {
            _locationSearchService = locationSearchService;
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _traineeshipSearchService = traineeshipSearchService;
            _addressSearchService = addressSearchService;
            _apprenticeshipSearchMapper = apprenticeshipSearchMapper;
            _traineeshipSearchMapper = traineeshipSearchMapper;
            _performanceCounterService = performanceCounterService;
            _configurationManager = configurationManager;
        }

        public LocationsViewModel FindLocation(string placeNameOrPostcode)
        {
            Logger.Debug("Calling SearchProvider to find the location for placename or postcode: {0}",
                placeNameOrPostcode);

            try
            {
                var locations = _locationSearchService.FindLocation(placeNameOrPostcode);

                if (locations == null)
                {
                    return new LocationsViewModel();
                }

                return new LocationsViewModel(
                    _apprenticeshipSearchMapper.Map<IEnumerable<Location>, IEnumerable<LocationViewModel>>(locations));
            }
            catch (CustomException e)
            {
                string message, errorMessage;

                switch (e.Code)
                {
                    case ErrorCodes.LocationLookupFailed:
                        errorMessage = string.Format("Location lookup failed for place name {0}", placeNameOrPostcode);
                        message = VacancySearchResultsPageMessages.LocationLookupFailed;
                        break;

                    default:
                        errorMessage = string.Format("Postcode lookup failed for postcode {0}", placeNameOrPostcode);
                        message = VacancySearchResultsPageMessages.PostcodeLookupFailed;
                        break;
                }

                Logger.Error(errorMessage, e);
                return new LocationsViewModel(message);
            }
            catch (Exception e)
            {
                var message = string.Format("Find location failed for placename or postcode {0}", placeNameOrPostcode);
                Logger.Error(message, e);
                throw;
            }
        }

        public ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search)
        {
            Logger.Debug("Calling SearchProvider to find apprenticeship vacancies.");

            var searchLocation = _apprenticeshipSearchMapper.Map<ApprenticeshipSearchViewModel, Location>(search);

            try
            {
                string vacancyReference;
                if (VacancyHelper.TryGetVacancyReference(search.Keywords, out vacancyReference))
                {
                    var searchParameters = new ApprenticeshipSearchParameters
                    {
                        VacancyReference = vacancyReference, PageNumber = 1, PageSize = 1
                    };
                    var searchResults = _apprenticeshipSearchService.Search(searchParameters);
                    //Expect only a single result. Any other number should be interpreted as no results
                    if (searchResults.Total == 1)
                    {
                        var exactMatchResponse = _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSummaryResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(searchResults);
                        exactMatchResponse.ExactMatchFound = true;
                        return exactMatchResponse;
                    }
                    if (searchResults.Total > 1)
                    {
                        Logger.Warn("{0} results found for Vacancy Reference Number {1} parsed from {2}. Expected 0 or 1", searchResults.Total, vacancyReference, search.Keywords);
                    }
                    var response = new ApprenticeshipSearchResponseViewModel
                    {
                        Vacancies = new List<ApprenticeshipVacancySummaryViewModel>(),
                        VacancySearch = search
                    };
                    return response;
                }

                var results = ProcessNationalAndNonNationalSearches(search, searchLocation);

                if (IsANewSearch(search))
                {
                    IncrementVacancySearchPerformanceCounter();
                }

                var nationalResults = results.Single(r => r.SearchParameters.VacancyLocationType == ApprenticeshipLocationType.National);

                var nonNationalResults = results.Single(r => r.SearchParameters.VacancyLocationType == ApprenticeshipLocationType.NonNational);

                var nationalResponse =
                    _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSummaryResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(
                        nationalResults);
                nationalResponse.VacancySearch = search;

                var nonNationlResponse =
                    _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSummaryResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(
                        nonNationalResults);
                nonNationlResponse.VacancySearch = search;

                if (search.LocationType == ApprenticeshipLocationType.NonNational)
                {
                    nonNationlResponse.TotalLocalHits = nonNationalResults.Total;
                    nonNationlResponse.TotalNationalHits = nationalResults.Total;
                    nonNationlResponse.PageSize = search.ResultsPerPage;

                    if (nonNationalResults.Total == 0 && nationalResults.Total > 0)
                    {
                        nonNationlResponse.Vacancies = nationalResponse.Vacancies;
                        nonNationlResponse.VacancySearch.LocationType = ApprenticeshipLocationType.National;
                        SetAggregationResults(nonNationlResponse, nationalResults.AggregationResults);
                    }
                    else
                    {
                        SetAggregationResults(nonNationlResponse, nonNationalResults.AggregationResults);
                    }

                    return nonNationlResponse;
                }

                nationalResponse.TotalLocalHits = nonNationalResults.Total;
                nationalResponse.TotalNationalHits = nationalResults.Total;
                nationalResponse.PageSize = search.ResultsPerPage;

                if (nationalResults.Total == 0 && nonNationalResults.Total != 0)
                {
                    nationalResponse.Vacancies = nonNationlResponse.Vacancies;
                    SetAggregationResults(nonNationlResponse, nonNationalResults.AggregationResults);
                }
                else
                {
                    SetAggregationResults(nonNationlResponse, nationalResults.AggregationResults);
                }

                return nationalResponse;
            }
            catch (CustomException ex)
            {
// ReSharper disable once FormatStringProblem
                Logger.Error("Find apprenticeship vacancies failed. Check inner details for more info", ex);
                return new ApprenticeshipSearchResponseViewModel(VacancySearchResultsPageMessages.VacancySearchFailed);
            }
            catch (Exception e)
            {
                Logger.Error("Find apprenticeship vacancies failed. Check inner details for more info", e);
                throw;
            }
        }

        private static void SetAggregationResults(ApprenticeshipSearchResponseViewModel response, IEnumerable<AggregationResult> aggregationResults)
        {
            if (aggregationResults == null || response.VacancySearch == null || response.VacancySearch.Categories == null) return;

            var aggregationResultsList = aggregationResults.ToList();

            foreach (var category in response.VacancySearch.Categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    var aggregationResult = aggregationResultsList.SingleOrDefault(ar => ar.Code == subCategory.CodeName);
                    if (aggregationResult != null) subCategory.Count = aggregationResult.Count;
                }
            }

            foreach (var category in response.VacancySearch.Categories.Where(c => c.SubCategories.Any(sc => sc.Count.HasValue)))
            {
                foreach (var subCategory in category.SubCategories)
                {
                    if (!subCategory.Count.HasValue) subCategory.Count = 0;
                }
            }
        }

        public TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search)
        {
            Logger.Debug("Calling SearchProvider to find traineeship vacancies.");

            var searchLocation = _traineeshipSearchMapper.Map<TraineeshipSearchViewModel, Location>(search);

            try
            {
                var searchRequest = new TraineeshipSearchParameters
                {
                    Location = searchLocation,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = search.SortType,
                };

                var searchResults = _traineeshipSearchService.Search(searchRequest);

                if (IsANewSearch(search))
                {
                    IncrementVacancySearchPerformanceCounter();
                }

                var searchResponse =
                    _traineeshipSearchMapper.Map<SearchResults<TraineeshipSummaryResponse, TraineeshipSearchParameters>, TraineeshipSearchResponseViewModel>(
                        searchResults);

                searchResponse.TotalHits = searchResults.Total;
                searchResponse.PageSize = search.ResultsPerPage;
                searchResponse.VacancySearch = search;

                return searchResponse;
            }
            catch (CustomException ex)
            {
                // ReSharper disable once FormatStringProblem
                Logger.Error("Find traineeship vacancies failed. Check inner details for more info", ex);
                return new TraineeshipSearchResponseViewModel(VacancySearchResultsPageMessages.VacancySearchFailed);
            }
            catch (Exception e)
            {
                Logger.Error("Find traineeship vacancies failed. Check inner details for more info", e);
                throw;
            }

        }

        public AddressSearchResult FindAddresses(string postcode)
        {
            Logger.Debug("Calling SearchProvider to find out the addresses for postcode {0}.", postcode);

            var addressSearchViewModel = new AddressSearchResult();

            try
            {
                IEnumerable<Address> addresses = _addressSearchService.FindAddress(postcode).OrderBy(x => x.Uprn);
                addressSearchViewModel.Addresses = _apprenticeshipSearchMapper.Map<IEnumerable<Address>, IEnumerable<AddressViewModel>>(addresses);
            }
            catch (CustomException e)
            {
                var message = string.Format("FindAddresses for postcode {0} failed.", postcode);
                Logger.Error(message, e);
                addressSearchViewModel.ErrorMessage = e.Message;
                addressSearchViewModel.HasError = true;
            }
            catch (Exception e)
            {
                var message = string.Format("FindAddresses for postcode {0} failed.", postcode);
                Logger.Error(message, e);
                throw;
            }

            return addressSearchViewModel;
        }

        private SearchResults<ApprenticeshipSummaryResponse, ApprenticeshipSearchParameters>[] ProcessNationalAndNonNationalSearches(
            ApprenticeshipSearchViewModel search, Location searchLocation)
        {
            VacancySortType nationalSortType, nonNationalSortType;

            if (search.LocationType == ApprenticeshipLocationType.National)
            {
                nationalSortType = search.SortType;
                nonNationalSortType = VacancySortType.Distance;
            }
            else
            {
                nonNationalSortType = search.SortType;
                nationalSortType = VacancySortType.ClosingDate;
            }
            
            var searchparameters = new List<ApprenticeshipSearchParameters>
            {
                new ApprenticeshipSearchParameters
                {
                    Keywords = search.Keywords,
                    Location = null,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    // SortType = string.IsNullOrWhiteSpace(search.Keywords) ? VacancySortType.ClosingDate : VacancySortType.Relevancy,
                    SortType = nationalSortType,
                    VacancyLocationType = ApprenticeshipLocationType.National,
                    ApprenticeshipLevel = search.ApprenticeshipLevel,
                    Sector = search.Category,
                    Frameworks = search.SubCategories
                },
                new ApprenticeshipSearchParameters
                {
                    Keywords = search.Keywords,
                    Location = searchLocation,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    // SortType = search.SortType,
                    SortType = nonNationalSortType,
                    VacancyLocationType = ApprenticeshipLocationType.NonNational,
                    ApprenticeshipLevel = search.ApprenticeshipLevel,
                    Sector = search.Category,
                    Frameworks = search.SubCategories
                }
            };

            var resultCollection = new ConcurrentBag<SearchResults<ApprenticeshipSummaryResponse, ApprenticeshipSearchParameters>>();
            Parallel.ForEach(searchparameters,
                parameters =>
                {
                    var searchResults = _apprenticeshipSearchService.Search(parameters);
                    resultCollection.Add(searchResults);
                });

            return resultCollection.ToArray();
        }

        private static bool IsANewSearch(VacancySearchViewModel search)
        {
            return search.SearchAction == SearchAction.Search && search.PageNumber == 1;
        }

        private void IncrementVacancySearchPerformanceCounter()
        {
            if(_configurationManager.GetCloudAppSetting<bool>("PerformanceCountersEnabled"))
            {
                _performanceCounterService.IncrementCounter(WebRolePerformanceCounterCategory, VacancySearchCounter);
            }
        }
    }
}