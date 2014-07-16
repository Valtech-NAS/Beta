namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Configuration;
    using Entities;
    using Rest;

    public class PostcodeService : RestService, IPostcodeLookupProvider
    {
        public PostcodeService(IConfigurationManager configurationManager) : 
            base(configurationManager.GetAppSetting("PostcodeServiceEndpoint")) { }

        public Location GetLocation(string postcode)
        {
            var result = GetPartialMatches(postcode).FirstOrDefault();

            return result == null
                ? null
                : new Location
                {
                    Name = postcode,
                    GeoPoint = new GeoPoint {Latitude = result.Latitude, Longitude = result.Longitude}
                };
        }

        private IEnumerable<PostcodeInfo> GetPartialMatches(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            var request = Create("postcodes?q={postcode}",
                new[] {new KeyValuePair<string, string>("postcode", postcode)});
            var postcodes = Execute<PostcodeInfoResult>(request);

            if (postcodes.Data != null && postcodes.Data.Result != null)
                return postcodes.Data.Result.AsEnumerable();

            return Enumerable.Empty<PostcodeInfo>();
        }
    }
}
