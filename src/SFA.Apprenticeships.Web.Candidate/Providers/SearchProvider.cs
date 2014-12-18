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
    using Constants.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Microsoft.WindowsAzure;
    using NLog;
    using Infrastructure.PerformanceCounters;
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
        private readonly IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail> _apprenticeshipSearchService;
        private readonly IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail> _traineeshipSearchService;
        private readonly IPerformanceCounterService _performanceCounterService;

        public SearchProvider(ILocationSearchService locationSearchService,
            IVacancySearchService<ApprenticeshipSummaryResponse, ApprenticeshipVacancyDetail> apprenticeshipSearchService,
            IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail> traineeshipSearchService,
            IAddressSearchService addressSearchService,
            IMapper apprenticeshipSearchMapper,
            IMapper traineeshipSearchMapper, 
            IPerformanceCounterService performanceCounterService)
        {
            _locationSearchService = locationSearchService;
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _traineeshipSearchService = traineeshipSearchService;
            _addressSearchService = addressSearchService;
            _apprenticeshipSearchMapper = apprenticeshipSearchMapper;
            _traineeshipSearchMapper = traineeshipSearchMapper;
            _performanceCounterService = performanceCounterService;
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
                var results = ProcessNationalAndNonNationalSearches(search, searchLocation);

                if (IsANewSearch(search))
                {
                    IncrementVacancySearchPerformanceCounter();
                }

                var nationalResults =
                    results[0].Results.Any(x => x.VacancyLocationType == ApprenticeshipLocationType.National)
                    ? results[0]
                    : results[1].Results.Any(x => x.VacancyLocationType == ApprenticeshipLocationType.National) ? results[1] : new SearchResults<ApprenticeshipSummaryResponse>(0, 1, null);

                var nonNationalResults = 
                    results[1].Results.Any(x => x.VacancyLocationType == ApprenticeshipLocationType.NonNational) 
                    ? results[1]
                    : results[0].Results.Any(x => x.VacancyLocationType == ApprenticeshipLocationType.NonNational) ? results[0] : new SearchResults<ApprenticeshipSummaryResponse>(0, 1, null);

                var nationalResponse =
                    _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSummaryResponse>, ApprenticeshipSearchResponseViewModel>(
                        nationalResults);

                var nonNationlResponse =
                    _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSummaryResponse>, ApprenticeshipSearchResponseViewModel>(
                        nonNationalResults);

                if (search.LocationType == ApprenticeshipLocationType.NonNational)
                {
                    nonNationlResponse.TotalLocalHits = nonNationalResults.Total;
                    nonNationlResponse.TotalNationalHits = nationalResults.Total;
                    nonNationlResponse.PageSize = search.ResultsPerPage;
                    nonNationlResponse.VacancySearch = search;

                    if (nonNationalResults.Total == 0 && nationalResults.Total > 0)
                    {
                        nonNationlResponse.Vacancies = nationalResponse.Vacancies;
                        nonNationlResponse.VacancySearch.LocationType = ApprenticeshipLocationType.National;
                    }

                    return nonNationlResponse;
                }

                nationalResponse.TotalLocalHits = nonNationalResults.Total;
                nationalResponse.TotalNationalHits = nationalResults.Total;
                nationalResponse.PageSize = search.ResultsPerPage;
                nationalResponse.VacancySearch = search;

                if (nationalResults.Total == 0 && nonNationalResults.Total != 0)
                {
                    nationalResponse.Vacancies = nonNationlResponse.Vacancies;                   
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

        public TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search)
        {
            Logger.Debug("Calling SearchProvider to find traineeship vacancies.");

            var searchLocation = _traineeshipSearchMapper.Map<TraineeshipSearchViewModel, Location>(search);

            try
            {
                var searchRequest = new SearchParameters
                {
                    Location = searchLocation,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = search.SortType,
                    VacancyLocationType = ApprenticeshipLocationType.NonNational
                };

                var searchResults = _traineeshipSearchService.Search(searchRequest);

                if (IsANewSearch(search))
                {
                    IncrementVacancySearchPerformanceCounter();
                }

                var searchResponse =
                    _traineeshipSearchMapper.Map<SearchResults<TraineeshipSummaryResponse>, TraineeshipSearchResponseViewModel>(
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

        public bool IsValidPostcode(string postcode)
        {
            Logger.Debug("Calling SearchProvider to find out if {0} is a valid postcode.", postcode);

            try
            {
                return LocationHelper.IsPostcode(postcode);
            }
            catch (Exception e)
            {
                var message = string.Format("IsValidPostcode failed for postcode {0}.", postcode);
                Logger.Error(message, e);
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

        private SearchResults<ApprenticeshipSummaryResponse>[] ProcessNationalAndNonNationalSearches(
            ApprenticeshipSearchViewModel search, Location searchLocation)
        {
            var searchparameters = new List<SearchParameters>
            {
                new SearchParameters
                {
                    Keywords = search.Keywords,
                    Location = null,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = string.IsNullOrWhiteSpace(search.Keywords) ? VacancySortType.ClosingDate : VacancySortType.Relevancy,
                    VacancyLocationType = ApprenticeshipLocationType.National
                },
                new SearchParameters
                {
                    Keywords = search.Keywords,
                    Location = searchLocation,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = search.SortType,
                    VacancyLocationType = ApprenticeshipLocationType.NonNational
                }
            };

            var resultCollection = new ConcurrentBag<SearchResults<ApprenticeshipSummaryResponse>>();
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
            bool performanceCountersEnabled;

            if (bool.TryParse(CloudConfigurationManager.GetSetting("PerformanceCountersEnabled"), out performanceCountersEnabled)
                && performanceCountersEnabled)
            {
                _performanceCounterService.IncrementCounter(WebRolePerformanceCounterCategory, VacancySearchCounter);
            }
        }
    }
}