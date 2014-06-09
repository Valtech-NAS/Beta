
namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private ILocationSearchService _locationSearchService;

        public SearchProvider(ILocationSearchService locationSearchService)
        {
            _locationSearchService = locationSearchService;
        }

        public IEnumerable<LocationViewModel> FindLocation(string placeNameOrPostcode)
        {
            throw new NotImplementedException();
        }
    }
}