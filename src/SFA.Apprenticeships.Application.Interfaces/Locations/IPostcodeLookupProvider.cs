using System;
using System.Collections.Generic;
using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    public interface IPostcodeLookupProvider
    {
        Domain.Entities.Locations.Location GetLocation(string postcode);

        IEnumerable<Address> FindAddresses(string postcode);
    }
}
