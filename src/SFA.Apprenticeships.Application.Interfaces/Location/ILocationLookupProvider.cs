namespace SFA.Apprenticeships.Application.Interfaces.Location
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Location;

    public interface ILocationLookupProvider
    {
        IEnumerable<LookupLocation> FindLocation(string placeName);
    }
}
