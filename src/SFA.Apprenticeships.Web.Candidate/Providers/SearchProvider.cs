namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using Application.Interfaces.Location;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancy;
    using Domain.Entities.Location;
    using Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Controllers;
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

        public VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search, LocationViewModel location, int pageSize)
        {
            var searchLocation = _mapper.Map<LocationViewModel, Location>(location);

            var searchResponse = _vacancySearchProvider.FindVacancies(search.JobTitle, search.Keywords, searchLocation, search.PageNumber, pageSize, search.WithinDistance);

            var vacancySearchResponseViewModel = _mapper.Map<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>(searchResponse);

            var pages = vacancySearchResponseViewModel.Pages(VacancySearchController.SearchPageSize);
            vacancySearchResponseViewModel.PrevPage = search.PageNumber == 1 ? 1 : search.PageNumber - 1;
            vacancySearchResponseViewModel.NextPage = search.PageNumber == pages ? pages : search.PageNumber + 1;
            vacancySearchResponseViewModel.VacancySearch = search;

            return vacancySearchResponseViewModel;
        }
    }
}