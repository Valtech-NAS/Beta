namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;
    using NLog;

    public class AddressSearchService : IAddressSearchService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAddressSearchProvider _addressSearchProvider;

        public AddressSearchService(IAddressSearchProvider addressSearchProvider)
        {
            _addressSearchProvider = addressSearchProvider;
        }

        public IEnumerable<Address> FindAddress(string postcode)
        {
            Logger.Debug("Calling AddressSearchService to find address for postcode={0}", postcode);
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            try
            {
                return _addressSearchProvider.FindAddress(postcode);
            }
            catch (Exception e)
            {
                var message = string.Format("FindAddress failed for postcode {0}.", postcode);
                Logger.Debug(message, e);
                throw new Domain.Entities.Exceptions.CustomException(
                    message, e, ErrorCodes.AddressSearchFailed);
            }
        }
    }
}
