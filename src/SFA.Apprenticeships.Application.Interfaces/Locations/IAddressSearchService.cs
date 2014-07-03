namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IAddressSearchService
    {
        IEnumerable<Address> FindAddress(string postcode);
    }
}
