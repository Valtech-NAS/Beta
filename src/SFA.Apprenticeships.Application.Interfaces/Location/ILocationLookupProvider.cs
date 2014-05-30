namespace SFA.Apprenticeships.Application.Interfaces.Location
{
    using System;
    using Domain.Entities.Location;

    public interface ILocationLookupProvider
    {
        Location GetLocation(string name);
    }
}
