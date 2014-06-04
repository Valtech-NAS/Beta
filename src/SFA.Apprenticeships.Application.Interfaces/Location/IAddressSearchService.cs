namespace SFA.Apprenticeships.Application.Interfaces.Location
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Location;

    public interface IAddressSearchService
    {
        IEnumerable<Address> FindAddress(string postcode);
    }
}
