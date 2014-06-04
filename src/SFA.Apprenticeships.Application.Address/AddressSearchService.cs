namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Interfaces.Services.Logging;
    using Interfaces.Location;
    using Domain.Entities.Location;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;
        private readonly ILoggingService _loggingService;

        public AddressSearchService(IPostcodeLookupProvider postcodeLookupProvider, ILoggingService loggingService)
        {
            Condition.Requires(postcodeLookupProvider, "postcodeLookupProvider").IsNotNull();
            Condition.Requires(loggingService, "loggingService").IsNotNull();

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
