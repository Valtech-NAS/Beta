namespace SFA.Apprenticeships.Infrastructure.LocationLookup
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Domain.Entities.Locations;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
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
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(LocationLookup));
            var term = placeName.ToLowerInvariant();

            Logger.Debug("Calling FindLocation for Term={0} on IndexName={1}", term, indexName);

            // NOTE: this function executes 3 Elasticsearch queries and then combines the results.

            // 1. Find place names that match the search term exactly.
            var exactMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q => q
                    .Match(m => m.OnField(f => f.Name).Query(term))
                )
                .From(0)
                .Size(maxResults));

            // 2. Find place names that are prefixed with the search term (autocomplete).
            var prefixMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q => q
                    .Prefix(p => p.OnField(n => n.Name).Value(term))
                )
                .From(0)
                .Size(maxResults));

            // 3. Find place names and counties by Levenshtein distance from the search term (http://en.wikipedia.org/wiki/Levenshtein_distance).
            var fuzzyMatchResults = client.Search<LocationLookup>(s => s
                .Index(indexName)
                .Query(q =>
                    q.Fuzzy(f => f.PrefixLength(1).OnField(n => n.Name).Value(term).Boost(2.0)) ||
                    q.Fuzzy(f => f.PrefixLength(1).OnField(n => n.County).Value(term).Boost(1.0))
                )
                .From(0)
                .Size(maxResults));

            // Prefer exact matches over prefix matches; prefer prefix matches over fuzzy matches.
            var results =
                exactMatchResults.Documents
                .Concat(prefixMatchResults.Documents)
                //.Concat(prefixMatchResults.Documents.Where(l => l.Name.ToLower().StartsWith(term.ToLower())).OrderBy(l => l.Name).ThenBy(l => l.County))
                .Concat(fuzzyMatchResults.Documents)
                .Distinct((new LocationLookupComparer()))
                .Take(maxResults)
                .ToList();

            Logger.Debug("{0} search results were returned", results.Count);

            return results.Select(location => new Location
            {
                Name = MakeName(location, results.Count),
                GeoPoint = new GeoPoint { Latitude = location.Latitude, Longitude = location.Longitude }
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