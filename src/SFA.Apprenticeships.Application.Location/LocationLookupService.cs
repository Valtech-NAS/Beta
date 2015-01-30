namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;
    using Interfaces.Logging;
    using ErrorCodes = Interfaces.Locations.ErrorCodes;

    public class LocationSearchService : ILocationSearchService
    {
        private readonly ILogService _logger;
        private readonly ILocationLookupProvider _locationLookupProvider;
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public LocationSearchService(ILocationLookupProvider locationLookupProvider, IPostcodeLookupProvider postcodeLookupProvider, ILogService logger)
        {
            _locationLookupProvider = locationLookupProvider;
            _postcodeLookupProvider = postcodeLookupProvider;
            _logger = logger;
        }

        public IEnumerable<Location> FindLocation(string placeNameOrPostcode)
        {
            Condition.Requires(placeNameOrPostcode, "placeNameOrPostcode").IsNotNullOrWhiteSpace();

            _logger.Debug("Calling LocationLookupService to find location for place name or postcode {0}.",
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
                _logger.Debug(message, e);
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
                _logger.Debug(message, e);
                throw new CustomException(message, e, ErrorCodes.PostcodeLookupFailed);
            }

            if (location == null)
            {
                _logger.Debug("Cannot find any match for place name or postcode {0}.", placeName);
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
