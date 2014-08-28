namespace SFA.Apprenticeships.Infrastructure.LocationLookup
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Domain.Entities.Locations;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Nest;
    using NLog;
    using GeoPoint = Domain.Entities.Locations.GeoPoint;

    internal class LocationLookupProvider : ILocationLookupProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public LocationLookupProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public IEnumerable<Location> FindLocation(string placeName, int maxResults = 50)
        {
            ElasticClient client = _elasticsearchClientFactory.GetElasticClient();
            string indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof (LocationLookup));
            string term = placeName.ToLowerInvariant();

            Logger.Debug("Calling find location for Term={0} on IndexName={1}", term, indexName);

            ISearchResponse<LocationLookup> exactMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q => q
                    .Match(m => m.OnField(f => f.Name).Query(term))
                )
                .From(0)
                .Size(maxResults));

            ISearchResponse<LocationLookup> fuzzyMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q =>
                    q.Fuzzy(f => f.PrefixLength(1).OnField(n => n.Name).Value(term).Boost(2.0)) ||
                    q.Fuzzy(f => f.PrefixLength(1).OnField(n => n.County).Value(term).Boost(1.0))
                )
                .From(0)
                .Size(maxResults));

            List<LocationLookup> results = exactMatchResults.Documents.Concat(fuzzyMatchResults.Documents)
                .Distinct((new LocationLookupComparer()))
                .Take(maxResults)
                .ToList();

            Logger.Debug("{0} search results were returned", results.Count);

            return results.Select(l => new Location
            {
                Name = MakeName(l, results.Count),
                GeoPoint = new GeoPoint {Latitude = l.Latitude, Longitude = l.Longitude}
            });
        }

        #region Helpers

        private static string MakeName(LocationLookup locationData, int total)
        {
            return total != 1 && locationData.Name != locationData.County
                ? string.Format("{0} ({1})", locationData.Name, locationData.County)
                : locationData.Name;
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
                return string.Format("{0},{1}", obj.Longitude, obj.Latitude).ToLower().GetHashCode();
            }
        }

        #endregion
    }
}