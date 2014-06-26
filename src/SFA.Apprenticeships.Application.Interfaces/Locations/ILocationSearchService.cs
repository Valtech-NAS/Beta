using System;
using System.Collections.Generic;

namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    public interface ILocationSearchService
    {
        /// <summary>
        /// returns locations that match the place name or postcode passed in
        /// </summary>
        /// <param name="placeNameOrPostcode">place name (town, city, village, county, etc.) or postcode</param>
        /// <returns>0..* matching locations</returns>
        IEnumerable<Domain.Entities.Locations.Location> FindLocation(string placeNameOrPostcode);
    }
}
