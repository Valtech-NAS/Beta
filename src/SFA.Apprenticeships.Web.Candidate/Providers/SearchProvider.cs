namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using Application.Interfaces.Location;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancy;
    using Domain.Entities.Location;
    using Domain.Interfaces.Mapping;
    using Controllers;
    using ViewModels.VacancySearch;

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

        public VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search, int pageSize)
        {
            var searchLocation = _mapper.Map<VacancySearchViewModel, Location>(search);

            var searchResponse = _vacancySearchProvider.FindVacancies(search.Keywords, searchLocation, search.PageNumber, pageSize, search.WithinDistance, search.SortType);

            var vacancySearchResponseViewModel = _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(searchResponse);
            vacancySearchResponseViewModel.PageSize = pageSize;
            vacancySearchResponseViewModel.VacancySearch = search;

            return vacancySearchResponseViewModel;
        }
    }
}