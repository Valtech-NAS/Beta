namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Common.Models.Common;
    using Constants.Pages;
    using Constants.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Mapping;
    using NLog;
    using ViewModels.Locations;
    using ViewModels.VacancySearch;
    using WebGrease.Css.Extensions;
    using ErrorCodes = Application.Interfaces.Locations.ErrorCodes;

    public class SearchProvider : ISearchProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAddressSearchService _addressSearchService;
        private readonly ILocationSearchService _locationSearchService;
        private readonly IMapper _mapper;
        private readonly IVacancySearchService _vacancySearchService;

        public SearchProvider(ILocationSearchService locationSearchService,
            IVacancySearchService vacancySearchService,
            IAddressSearchService addressSearchService,
            IMapper mapper)
        {
            _locationSearchService = locationSearchService;
            _vacancySearchService = vacancySearchService;
            _addressSearchService = addressSearchService;
            _mapper = mapper;
        }

        public LocationsViewModel FindLocation(string placeNameOrPostcode)
        {
            Logger.Debug("Calling SearchProvider to find the location for placename or postcode: {0}",
                placeNameOrPostcode);

            try
            {
                IEnumerable<Location> locations = _locationSearchService.FindLocation(placeNameOrPostcode);

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

                Logger.ErrorException(errorMessage, e);
                return new LocationsViewModel(message);
            }
            catch (Exception e)
            {
                var message = string.Format("Find location failed for placename or postcode {0}", placeNameOrPostcode);
                Logger.ErrorException(message, e);
                throw;
            }
        }

        public VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search, int pageSize)
        {
            Logger.Debug("Calling SearchProvider to find vacancies.");

            var searchLocation = _mapper.Map<VacancySearchViewModel, Location>(search);

            try
            {
                SearchResults<VacancySummaryResponse> searchResponse = _vacancySearchService.Search(search.Keywords,
                    searchLocation, search.PageNumber,
                    pageSize, search.WithinDistance, search.SortType);

                VacancySearchResponseViewModel vacancySearchResponseViewModel =
                    _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(searchResponse);

                switch (search.LocationType)
                {
                    case VacancyLocationType.Local:
                        vacancySearchResponseViewModel.Vacancies.ForEach(
                            r => r.VacancyLocationType = VacancyLocationType.Local);
                        break;
                    case VacancyLocationType.Nationwide:
                        vacancySearchResponseViewModel.Vacancies.ForEach(
                            r => r.VacancyLocationType = VacancyLocationType.Nationwide);
                        break;
                }

                vacancySearchResponseViewModel.TotalLocalHits = searchResponse.Total;
                vacancySearchResponseViewModel.TotalNationalHits = searchResponse.Total;

                vacancySearchResponseViewModel.PageSize = pageSize;
                vacancySearchResponseViewModel.VacancySearch = search;

                return vacancySearchResponseViewModel;
            }
            catch (CustomException ex)
            {
                Logger.ErrorException("Find vacancies failed. Check inner details for more info", ex);
                return new VacancySearchResponseViewModel(VacancySearchResultsPageMessages.VacancySearchFailed);
            }
            catch (Exception e)
            {
                Logger.ErrorException("Find vacancies failed. Check inner details for more info", e);
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
                Logger.ErrorException(message, e);
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
                addressSearchViewModel.Addresses = addresses != null
                    ? _mapper.Map<IEnumerable<Address>, IEnumerable<AddressViewModel>>(addresses)
                    : new AddressViewModel[] {};
            }
            catch (CustomException e)
            {
                var message = string.Format("FindAddresses for postcode {0} failed.", postcode);
                Logger.ErrorException(message, e);
                addressSearchViewModel.ErrorMessage = e.Message;
                addressSearchViewModel.HasError = true;
            }
            catch (Exception e)
            {
                var message = string.Format("FindAddresses for postcode {0} failed.", postcode);
                Logger.ErrorException(message, e);
                throw;
            }

            return addressSearchViewModel;
        }
    }
}