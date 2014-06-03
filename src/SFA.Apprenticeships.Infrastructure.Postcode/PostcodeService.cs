namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Rest;
    using CuttingEdge.Conditions;
    using Entities;
    using Application.Interfaces.Location;
    using Domain.Entities.Location;

    /// <summary>
    /// <add key="PostcodeServiceEndpoint" value="http://api.postcodes.io" />
    /// </summary>
    public class PostcodeService : RestService, IPostcodeLookupProvider
    {
        public PostcodeService(string baseUrl) : base(baseUrl) {}

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

        public IEnumerable<Address> FindAddresses(string postcode)
        {
            //todo: needs an address lookup
            throw new NotImplementedException();
        }

        private IEnumerable<PostcodeInfo> GetPartialMatches(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrEmpty();

            var request = Create("postcodes?q={postcode}", new[] {new KeyValuePair<string, string>("postcode", postcode)});
            var postcodes = Execute<PostcodeInfoResult>(request);

            if (postcodes.Data != null && postcodes.Data.Result != null)
                return postcodes.Data.Result.AsEnumerable();

            return Enumerable.Empty<PostcodeInfo>();
        }
    }
}
