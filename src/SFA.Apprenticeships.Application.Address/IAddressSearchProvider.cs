namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IAddressSearchProvider
    {
        IEnumerable<Address> FindAddress(string postcode);
    }
}
