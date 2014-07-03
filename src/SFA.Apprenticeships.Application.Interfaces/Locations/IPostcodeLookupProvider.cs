namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IPostcodeLookupProvider
    {
        Location GetLocation(string postcode);

        IEnumerable<Address> FindAddresses(string postcode);
    }
}
