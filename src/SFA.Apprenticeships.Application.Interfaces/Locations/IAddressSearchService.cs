using System;
using System.Collections.Generic;
using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    public interface IAddressSearchService
    {
        IEnumerable<Address> FindAddress(string postcode);
    }
}
