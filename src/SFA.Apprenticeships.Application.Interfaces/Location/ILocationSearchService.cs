namespace SFA.Apprenticeships.Application.Interfaces.Location
{
    using System;
    using Domain.Entities.Location;

    public interface ILocationSearchService
    {
        Location GetLocation(string name);
    }
}
