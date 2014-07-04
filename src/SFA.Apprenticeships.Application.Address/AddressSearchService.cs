namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public AddressSearchService(IPostcodeLookupProvider postcodeLookupProvider)
        {
            _postcodeLookupProvider = postcodeLookupProvider;
        }

        public IEnumerable<Address> FindAddress(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            if (!LocationHelper.IsPostcode(postcode))
                throw new ArgumentException("Invalid postcode specified");

            return _postcodeLookupProvider.FindAddresses(postcode);
        }
    }
}
