namespace SFA.Apprenticeships.Application.Address
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IAddressSearchProvider _addressSearchProvider;

        public AddressSearchService(IAddressSearchProvider addressSearchProvider)
        {
            _addressSearchProvider = addressSearchProvider;
        }

        public IEnumerable<Address> FindAddress(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            if (!LocationHelper.IsPostcode(postcode))
                throw new ArgumentException("Invalid postcode specified");

            try
            {
                return _addressSearchProvider.FindAddress(postcode);
            }
            catch (Exception e)
            {
                var message = string.Format("FindAddress failed for postcode {0}.", postcode);
                throw new Domain.Entities.Exceptions.CustomException(
                    message, e, ErrorCodes.AddressSearchFailed);
            }
        }
    }
}
