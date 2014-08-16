namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface ILocationLookupProvider
    {
        IEnumerable<Location> FindLocation(string placeName, int maxResults = 50);
    }
}
