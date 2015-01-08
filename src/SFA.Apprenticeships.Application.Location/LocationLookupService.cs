namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;
    using NLog;
    using ErrorCodes = Interfaces.Locations.ErrorCodes;

    public class LocationSearchService : ILocationSearchService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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

            Logger.Debug("Calling LocationLookupService to find location for place name or postcode {0}.",
                placeNameOrPostcode);

            if (LocationHelper.IsPostcode(placeNameOrPostcode) || LocationHelper.IsPartialPostcode(placeNameOrPostcode))
            {
                return FindLocationByPostcode(placeNameOrPostcode);
            }

            return FindLocationByPlaceName(placeNameOrPostcode);
        }

        private IEnumerable<Location> FindLocationByPlaceName(string postcode)
        {
            try
            {
                return _locationLookupProvider.FindLocation(postcode);
            }
            catch (Exception e)
            {
                const string message = "Location lookup failed.";
                Logger.Debug(message, e);
                throw new CustomException(
                    message, e, ErrorCodes.LocationLookupFailed);
            }
        }

        private IEnumerable<Location> FindLocationByPostcode(string placeName)
        {
            Location location;

            try
            {
                location = _postcodeLookupProvider.GetLocation(placeName);
            }
            catch (Exception e)
            {
                var message = string.Format("Postcode lookup failed for postcode {0}.", placeName);
                Logger.Debug(message, e);
                throw new CustomException(message, e, ErrorCodes.PostcodeLookupFailed);
            }

            if (location == null)
            {
                Logger.Debug("Cannot find any match for place name or postcode {0}.", placeName);
                return null; // no match
            }

            return new[]
            {
                new Location
                {
                    GeoPoint = location.GeoPoint,
                    Name = location.Name
                }
            };
        }
    }
}
