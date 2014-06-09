
namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Location;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private ILocationSearchService _locationSearchService;
        private readonly IVacancySearchProvider _vacancySearchProvider;
        private readonly IMapper _mapper;


        public SearchProvider(ILocationSearchService locationSearchService)
        {
            _locationSearchService = locationSearchService;
        }

        public SearchProvider(IVacancySearchProvider vacancySearchProvider, IMapper mapper)
        {
            _vacancySearchProvider = vacancySearchProvider;
            _mapper = mapper;
        }

        public IEnumerable<LocationViewModel> FindLocation(string placeNameOrPostcode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VacancySearchResponseViewModel> FindVacacnies(LocationViewModel location, int radius)
        {
            var searchLocation = new Location()
            {
                Name = "Search Loction",
                GeoPoint = new GeoPoint()
                {
                    Latitute = location.Latitude,
                    Longitude = location.Longitude
                }
            };
            
            var searchResponse = _vacancySearchProvider.FindVacancies(searchLocation, radius);
            return null;
        }
    }
}