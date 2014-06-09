namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Location;
    using Interfaces.Location;

    public class LocationSearchService : ILocationSearchService
    {
        private readonly ILocationLookupProvider _locationLookupProvider;
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public LocationSearchService(ILocationLookupProvider locationLookupProvider, IPostcodeLookupProvider postcodeLookupProvider)
        {
            _locationLookupProvider = locationLookupProvider;
            _postcodeLookupProvider = postcodeLookupProvider;
        }

        public IEnumerable<Location> FindLocation(string placeNameOrPostcode)
        {
            Condition.Requires(placeNameOrPostcode, "placeNameOrPostcode").IsNotNullOrWhiteSpace();

            if (LocationHelper.IsPartialPostcode(placeNameOrPostcode))
            {
                var location = _postcodeLookupProvider.GetLocation(placeNameOrPostcode);

                if (location == null) return null; // no match

                return new[]
                {
                    new Location
                    {
                        GeoPoint = location.GeoPoint,
                        Name = location.Name
                    }
                };
            }

            return _locationLookupProvider.FindLocation(placeNameOrPostcode);
        }
    }
}
