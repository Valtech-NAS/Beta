namespace SFA.Apprenticeships.Application.Interfaces.Location
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Location;

    public interface IPostcodeLookupProvider
    {
        Location GetLocation(string postcode);
        IEnumerable<Address> FindAddresses(string postcode);
    }
}
