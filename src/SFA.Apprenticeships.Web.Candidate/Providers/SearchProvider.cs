namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using Application.Interfaces.Location;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancy;
    using Domain.Entities.Location;
    using Domain.Interfaces.Mapping;
    using Web.Candidate.ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private readonly ILocationSearchService _locationSearchService;
        private readonly IVacancySearchProvider _vacancySearchProvider;
        private readonly IMapper _mapper;

        public SearchProvider(ILocationSearchService locationSearchService, IVacancySearchProvider vacancySearchProvider, IMapper mapper)
        {
            _locationSearchService = locationSearchService;
            _vacancySearchProvider = vacancySearchProvider;
            _mapper = mapper;
        }

        public IEnumerable<LocationViewModel> FindLocation(string placeNameOrPostcode)
        {
            var locations = _locationSearchService.FindLocation(placeNameOrPostcode);

            if (locations != null)
            {
                return _mapper.Map<IEnumerable<Location>, IEnumerable<LocationViewModel>>(locations);
            }

            return new LocationViewModel[]{};
        }

        public VacancySearchResponseViewModel FindVacancies(string jobTitle, string keywords, LocationViewModel location, int pageNumber, int pageSize, int searchRadius)
        {
            var searchLocation = _mapper.Map<LocationViewModel, Location>(location);

            var searchResponse = _vacancySearchProvider.FindVacancies(jobTitle, keywords, searchLocation, pageNumber, pageSize, searchRadius);

            var vacancySearchResponseViewModel = _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(searchResponse);

            return vacancySearchResponseViewModel;
        }
    }
}