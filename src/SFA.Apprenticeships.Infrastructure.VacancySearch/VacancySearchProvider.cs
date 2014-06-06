namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System.Collections.Generic;
    using Nest;
    using Application.Interfaces.Vacancy;
    using Domain.Entities.Location;
    using Domain.Entities.Vacancy;
    using Elastic.Common.Configuration;

    public class VacancySearchProvider : IVacancySearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public VacancySearchProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public IEnumerable<VacancySummary> FindVacancies(Location location, int radius)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof (Elastic.Common.Entities.VacancySummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.VacancySummary));

            var query = client.Search<VacancySummary>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);
                s.From(0);
                s.Take(10);
                s.SortGeoDistance(g =>
                {
                    g.PinTo(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                     .Unit(GeoUnit.mi).OnField(f => f.Location);
                    return g;
                });

                return s;
            });

            return query.Documents;
        }
    }
}
