namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;

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

            if (LocationHelper.IsPostcode(placeNameOrPostcode))
            {
                Location location;

                try
                {
                    location = _postcodeLookupProvider.GetLocation(placeNameOrPostcode);
                }
                catch (Exception e)
                {
                    var message = string.Format("Postcode lookup failed for postcode {0}.", placeNameOrPostcode);
                    throw new Domain.Entities.Exceptions.CustomException(
                        message, e, ErrorCodes.PostcodeLookupFailed);
                }

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

            try
            {
                return _locationLookupProvider.FindLocation(placeNameOrPostcode);
            }
            catch (Exception e)
            {
                throw new Domain.Entities.Exceptions.CustomException(
                    "Location lookup failed.", e, ErrorCodes.LocationLookupFailed);
            }
        }
    }
}
