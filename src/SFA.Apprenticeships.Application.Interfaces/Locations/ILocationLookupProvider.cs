using System;
using System.Collections.Generic;

namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    public interface ILocationLookupProvider
    {
        IEnumerable<Domain.Entities.Locations.Location> FindLocation(string placeName, int maxResults = 50); //todo: replace with paging request
    }
}
