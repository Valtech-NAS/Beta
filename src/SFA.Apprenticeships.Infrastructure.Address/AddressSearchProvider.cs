namespace SFA.Apprenticeships.Infrastructure.Address
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Locations;
    using Domain.Entities.Locations;
    using CuttingEdge.Conditions;

    public class AddressSearchProvider : IAddressSearchProvider
    {
        public IEnumerable<Address> FindAddresses(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            //todo: call ES here...
            throw new NotImplementedException();
        }
    }
}
