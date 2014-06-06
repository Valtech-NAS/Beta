namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System;
    using System.Collections.Generic;
    using Nest;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Location;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;

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

            var query = client.Search<VacancySummary>(s =>
            {
                s.Index("vacancysummaries_test");
                s.Type("vacancy");
                s.From(0);
                s.Take(10);
                /*s.SortGeoDistance(g =>
                {
                    g.PinTo(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                     .Unit(GeoUnit.mi).OnField(f => f.Location);
                    return g;
                });*/
                return s;
            });

            return query.Documents;
        }
    }
}
