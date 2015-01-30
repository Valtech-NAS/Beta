namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;
    using Interfaces.Logging;
    using ErrorCodes = Interfaces.Locations.ErrorCodes;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly ILogService _logger;
        private readonly IAddressSearchProvider _addressSearchProvider;

        public AddressSearchService(IAddressSearchProvider addressSearchProvider, ILogService logger)
        {
            _addressSearchProvider = addressSearchProvider;
            _logger = logger;
        }

        public IEnumerable<Address> FindAddress(string postcode)
        {
            _logger.Debug("Calling AddressSearchService to find address for postcode={0}", postcode);
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            try
            {
                return _addressSearchProvider.FindAddress(postcode);
            }
            catch (Exception e)
            {
                var message = string.Format("FindAddress failed for postcode {0}.", postcode);
                _logger.Debug(message, e);
                throw new CustomException(message, e, ErrorCodes.AddressSearchFailed);
            }
        }
    }
}
