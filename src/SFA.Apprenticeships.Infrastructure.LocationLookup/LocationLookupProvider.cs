namespace SFA.Apprenticeships.Infrastructure.LocationLookup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Location;
    using Domain.Entities.Location;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;

    internal class LocationLookupProvider : ILocationLookupProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public LocationLookupProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public IEnumerable<Location> FindLocation(string placeName, int maxResults = 50)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(LocationLookup));
            var term = placeName.ToLowerInvariant();

            var exactMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q => q
                    .MultiMatch(m => m
                        .OnFields(f => f.County, f => f.Name)
                        .QueryString(term))
                )
                .From(0)
                .Size(maxResults));

            var fuzzyMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q =>
                    q.Fuzzy(f => f.OnField(n => n.Name).Value(term).Boost(2.0)) ||
                    q.Fuzzy(f => f.OnField(n => n.County).Value(term).Boost(1.0))
                )
                .From(0)
                .Size(maxResults));

            var results = exactMatchResults.Documents.Concat(fuzzyMatchResults.Documents)
                .Distinct((new LocationLookupComparer()))
                .Take(maxResults)
                .ToList();

            return results.Select(l => new Location
            {
                Name = MakeName(l, results.Count),
                GeoPoint = new Domain.Entities.Location.GeoPoint { Latitute = l.Latitude, Longitude = l.Longitude }
            });
        }

        #region Helpers
        private static string MakeName(LocationLookup locationData, int total)
        {
            return total != 1 ?
                string.Format("{0} ({1})", locationData.Name, locationData.County) :
                locationData.Name;
        }

        private class LocationLookupComparer : IEqualityComparer<LocationLookup>
        {
            public bool Equals(LocationLookup g1, LocationLookup g2)
            {
                return g1.Latitude.Equals(g2.Latitude) &&
                       g1.Longitude.Equals(g2.Longitude);
            }

            public int GetHashCode(LocationLookup obj)
            {
                return string.Format("{0},{1}", obj.Longitude, obj.Latitude).ToLower().GetHashCode(); ;
            }
        }
        #endregion
    }
}
