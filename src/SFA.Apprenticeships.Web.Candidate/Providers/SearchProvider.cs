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
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using Microsoft.WindowsAzure;
    using NLog;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;
    using ViewModels.Locations;
    using ViewModels.VacancySearch;
    using ErrorCodes = Application.Interfaces.Locations.ErrorCodes;

    public class SearchProvider : ISearchProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAddressSearchService _addressSearchService;
        private readonly ILocationSearchService _locationSearchService;
        private readonly IMapper _mapper;
        private readonly IVacancySearchService _vacancySearchService;
        private readonly IPerformanceCounterService _performanceCounterService;

        public SearchProvider(ILocationSearchService locationSearchService,
            IVacancySearchService vacancySearchService,
            IAddressSearchService addressSearchService,
            IMapper mapper, 
            IPerformanceCounterService performanceCounterService)
        {
            _locationSearchService = locationSearchService;
            _vacancySearchService = vacancySearchService;
            _addressSearchService = addressSearchService;
            _mapper = mapper;
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
                    _mapper.Map<IEnumerable<Location>, IEnumerable<LocationViewModel>>(locations));
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

        public VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search)
        {
            Logger.Debug("Calling SearchProvider to find vacancies.");

            var searchLocation = _mapper.Map<VacancySearchViewModel, Location>(search);

            try
            {
                var results = ProcessNationalAndNonNationalSearches(search, searchLocation);

                if (IsANewSearch(search))
                {
                    IncrementVacancySearchPerformanceCounter();    
                }

                var nationalResults =
                    results[0].Results.Any(x => x.VacancyLocationType == VacancyLocationType.National)
                    ? results[0]
                    : results[1].Results.Any(x => x.VacancyLocationType == VacancyLocationType.National) ? results[1] : new SearchResults<VacancySummaryResponse>(0, 1, null);

                var nonNationalResults = 
                    results[1].Results.Any(x => x.VacancyLocationType == VacancyLocationType.NonNational) 
                    ? results[1]
                    : results[0].Results.Any(x => x.VacancyLocationType == VacancyLocationType.NonNational) ? results[0] : new SearchResults<VacancySummaryResponse>(0, 1, null);

                var nationalResponse =
                    _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(
                        nationalResults);

                var nonNationlResponse =
                    _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(
                        nonNationalResults);

                if (search.LocationType == VacancyLocationType.NonNational)
                {
                    nonNationlResponse.TotalLocalHits = nonNationalResults.Total;
                    nonNationlResponse.TotalNationalHits = nationalResults.Total;
                    nonNationlResponse.PageSize = search.ResultsPerPage;
                    nonNationlResponse.VacancySearch = search;

                    if (nonNationalResults.Total == 0 && nationalResults.Total != 0)
                    {
                        nonNationlResponse.Vacancies = nationalResponse.Vacancies;
                        nonNationlResponse.VacancySearch.SortType = VacancySortType.ClosingDate;
                        nonNationlResponse.VacancySearch.LocationType = VacancyLocationType.National;
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
                Logger.Error("Find vacancies failed. Check inner details for more info", ex);
                return new VacancySearchResponseViewModel(VacancySearchResultsPageMessages.VacancySearchFailed);
            }
            catch (Exception e)
            {
                Logger.Error("Find vacancies failed. Check inner details for more info", e);
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
                addressSearchViewModel.Addresses = _mapper.Map<IEnumerable<Address>, IEnumerable<AddressViewModel>>(addresses);
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

        private SearchResults<VacancySummaryResponse>[] ProcessNationalAndNonNationalSearches(
            VacancySearchViewModel search, Location searchLocation)
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
                    VacancyLocationType = VacancyLocationType.National
                },
                new SearchParameters
                {
                    Keywords = search.Keywords,
                    Location = searchLocation,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = search.SortType,
                    VacancyLocationType = VacancyLocationType.NonNational
                }
            };

            var resultCollection = new ConcurrentBag<SearchResults<VacancySummaryResponse>>();
            Parallel.ForEach(searchparameters,
                parameters =>
                {
                    var searchResults = _vacancySearchService.Search(parameters);
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
                _performanceCounterService.IncrementVacancySearchPerformanceCounter();
            }
        }
    }
}