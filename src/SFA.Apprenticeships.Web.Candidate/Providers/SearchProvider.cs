namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Location;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancy;
    using Domain.Entities.Location;
    using Domain.Entities.Vacancy;
    using Domain.Interfaces.Mapping;
    using Web.Candidate.ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private ILocationSearchService _locationSearchService;
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
            throw new NotImplementedException();
        }

        public VacancySearchResponseViewModel FindVacancies(LocationViewModel location, int radius)
        {
            var searchLocation = _mapper.Map<LocationViewModel, Location>(location);

            var searchResponse = _vacancySearchProvider.FindVacancies(searchLocation, radius);

            var vacancySearchResponseViewModel = _mapper.Map<SearchResults<VacancySummary>, VacancySearchResponseViewModel>(searchResponse);

            return vacancySearchResponseViewModel;
        }
    }
}