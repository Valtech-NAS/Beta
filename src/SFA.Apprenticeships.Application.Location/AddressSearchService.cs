namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Location;
    using Domain.Entities.Location;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public AddressSearchService(IPostcodeLookupProvider postcodeLookupProvider)
        {
            _postcodeLookupProvider = postcodeLookupProvider;
        }

        public IEnumerable<Address> FindAddresses(string postcode)
        {
            return _postcodeLookupProvider.FindAddresses(postcode);
        }
    }
}
