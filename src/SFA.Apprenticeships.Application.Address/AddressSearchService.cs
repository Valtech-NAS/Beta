namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Logging;
    using Interfaces.Locations;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;
        private readonly ILoggingService _loggingService;

        public AddressSearchService(IPostcodeLookupProvider postcodeLookupProvider, ILoggingService loggingService)
        {
            _postcodeLookupProvider = postcodeLookupProvider;
            _loggingService = loggingService;
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
