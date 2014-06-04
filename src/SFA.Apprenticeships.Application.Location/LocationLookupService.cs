namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Location;
    using Domain.Interfaces.Services.Logging;
    using Interfaces.Location;

    public class LocationSearchService : ILocationSearchService
    {
        private readonly ILocationLookupProvider _locationLookupProvider;
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;
        private readonly ILoggingService _loggingService;

        public LocationSearchService(ILocationLookupProvider locationLookupProvider, IPostcodeLookupProvider postcodeLookupProvider, ILoggingService loggingService)
        {
            Condition.Requires(locationLookupProvider, "locationLookupProvider").IsNotNull();
            Condition.Requires(postcodeLookupProvider, "postcodeLookupProvider").IsNotNull();
            Condition.Requires(loggingService, "loggingService").IsNotNull();

            _locationLookupProvider = locationLookupProvider;
            _postcodeLookupProvider = postcodeLookupProvider;
            _loggingService = loggingService;
        }

        public IEnumerable<LookupLocation> FindLocation(string placeNameOrPostcode)
        {
            Condition.Requires(placeNameOrPostcode, "placeNameOrPostcode").IsNotNullOrWhiteSpace();

            if (LocationHelper.IsPostcode(placeNameOrPostcode))
            {
                var location = _postcodeLookupProvider.GetLocation(placeNameOrPostcode);

                if (location == null) return null; // no match

                return new[]
                {
                    new LookupLocation
                    {
                        GeoPoint = location.GeoPoint,
                        Name = location.Name,
                        Type = "Postcode"
                    }
                };
            }

            return _locationLookupProvider.FindLocation(placeNameOrPostcode);
        }
    }
}
