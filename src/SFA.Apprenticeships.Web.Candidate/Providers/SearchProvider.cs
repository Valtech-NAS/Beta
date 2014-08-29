// TODO: AG: US333: logging (should be in which layer?).

namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Constants.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Mapping;
    using ViewModels.Locations;
    using ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private readonly ILocationSearchService _locationSearchService;
        private readonly IVacancySearchService _vacancySearchService;
        private readonly IAddressSearchService _addressSearchService;
        private readonly IMapper _mapper;

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
                string message;

                switch (e.Code)
                {
                    case Application.Interfaces.Locations.ErrorCodes.LocationLookupFailed:
                        message = VacancySearchResultsPageMessages.LocationLookupFailed;
                        break;

                    default:
                        message = VacancySearchResultsPageMessages.PostcodeLookupFailed;
                        break;
                }

                return new LocationsViewModel(message);
            }
        }

        public VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search, int pageSize)
        {
            var searchLocation = _mapper.Map<VacancySearchViewModel, Location>(search);

            try
            {
                var searchResponse = _vacancySearchService.Search(search.Keywords, searchLocation, search.PageNumber,
                    pageSize, search.WithinDistance, search.SortType);

                var vacancySearchResponseViewModel =
                    _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(searchResponse);

                vacancySearchResponseViewModel.PageSize = pageSize;
                vacancySearchResponseViewModel.VacancySearch = search;

                return vacancySearchResponseViewModel;
            }
            catch (CustomException)
            {
                return new VacancySearchResponseViewModel(VacancySearchResultsPageMessages.VacanciesSearchFailed);
            }
        }

        public bool IsValidPostcode(string postcode)
        {
            return LocationHelper.IsPostcode(postcode);
        }

        public IEnumerable<AddressViewModel> FindAddresses(string postcode)
        {
            var addresses = _addressSearchService.FindAddress(postcode);
            return addresses != null ? _mapper.Map<IEnumerable<Address>, IEnumerable<AddressViewModel>>(addresses) : new AddressViewModel[] { };
        }
    }
}
