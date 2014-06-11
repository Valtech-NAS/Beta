namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System.Globalization;
    using System.Linq;
    using Nest;
    using Application.Interfaces.Vacancy;
    using Application.Interfaces.Search;
    using Domain.Entities.Location;
    using Elastic.Common.Configuration;

    public class VacancySearchProvider : IVacancySearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public VacancySearchProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public SearchResults<VacancySummaryResponse> FindVacancies(string jobTitle, string keywords, Location location, int pageNumber, int pageSize, int searchRadius)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof (Elastic.Common.Entities.VacancySummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.VacancySummary));

            var search = client.Search<VacancySummaryResponse>(s =>
            {              
                s.Index(indexName);
                s.Type(documentTypeName);     
                s.Skip((pageNumber - 1) * pageSize);
                s.Take(pageSize);
                s.SortGeoDistance(g =>
                {
                    g.PinTo(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                     .Unit(GeoUnit.mi).OnField(f => f.Location);
                    return g;
                }).Filter(f => f.GeoDistance(vs => vs.Location, descriptor => 
                    descriptor
                    .Location(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                    .Distance(searchRadius, GeoUnit.mi)));

                s.Query(q =>
                {
                    BaseQuery query = null;
                    if (!string.IsNullOrEmpty(jobTitle))
                    {
                        query &= q.QueryString(m => m.OnField(f => f.Title).Query(jobTitle));
                    }
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        query &= q.QueryString(m => m.OnField(f => f.Description).Query(keywords));

                    }
                    return query;
                });

                return s;
            });

            var responses = search.Documents;
            responses.ToList()
                .ForEach(
                    r =>
                        r.Distance =
                        double.Parse(
                            search.Hits.Hits.First(h => h.Id == r.Id.ToString(CultureInfo.InvariantCulture))
                                .Sorts.First()
                                .ToString()));
            var results = new SearchResults<VacancySummaryResponse>(search.Total, pageNumber, responses);
            return results;
        }
    }
}
