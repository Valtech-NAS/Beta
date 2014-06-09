
namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private readonly ILocationSearchService _locationSearchService;

        public SearchProvider(ILocationSearchService locationSearchService)
        {
            _locationSearchService = locationSearchService;
        }

        public IEnumerable<LocationViewModel> FindLocation(string placeNameOrPostcode)
        {
            var locations = _locationSearchService.FindLocation(placeNameOrPostcode);

            if (locations != null)
            {
                return locations
                    .Select(
                        location =>
                            new LocationViewModel
                            {
                                Latitude = location.GeoPoint.Latitute,
                                Longitude = location.GeoPoint.Longitude,
                                Name = location.Name,
                            });
            }

            return new LocationViewModel[]{};
        }
    }
}