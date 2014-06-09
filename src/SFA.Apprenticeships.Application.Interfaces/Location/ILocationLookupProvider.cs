namespace SFA.Apprenticeships.Application.Interfaces.Location
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Location;

    public interface ILocationLookupProvider
    {
        IEnumerable<Location> FindLocation(string placeName, int maxResults = 50); //todo: replace with paging request
    }
}
