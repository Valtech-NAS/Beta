namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using Domain.Entities.Locations;

    public interface IPostcodeLookupProvider
    {
        Location GetLocation(string postcode);
    }
}
