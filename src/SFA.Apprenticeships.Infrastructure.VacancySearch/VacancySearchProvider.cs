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

        public SearchResults<VacancySummaryResponse> FindVacancies(string keywords, 
                                                                    Location location, 
                                                                    int pageNumber, 
                                                                    int pageSize, 
                                                                    int searchRadius,
                                                                    VacancySortType sortType)
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

                switch (sortType)
                {
                    case VacancySortType.Distance:
                        s.SortGeoDistance(g =>
                        {
                            g.PinTo(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                             .Unit(GeoUnit.mi).OnField(f => f.Location);
                            return g;
                        });
                        break;
                    case VacancySortType.ClosingDate:
                        s.Sort(v => v.OnField(f => f.ClosingDate).Descending());
                        break;
                    case VacancySortType.Relevancy:
                        //No sort - let Elasticsearch score based on query.
                        break;
                }

                if (location != null)
                {
                    //Needed for both Distance and ClosingDate
                    s.Filter(f => f.GeoDistance(
                                        vs => vs.Location, 
                                        descriptor =>
                                            descriptor
                                            .Location(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                                            .Distance(searchRadius, GeoUnit.mi)));
                }

                if (!string.IsNullOrEmpty(keywords))
                {
                    s.Query(q =>
                    {
                        BaseQuery query = q.Fuzzy(f => f.MinSimilarity(5).PrefixLength(1).OnField(n => n.Title).Value(keywords).Boost(2.0))
                                            ||
                                          q.Fuzzy(f => f.MinSimilarity(2).PrefixLength(1).OnField(n => n.Description).Value(keywords).Boost(1.0));
                        //query = q.Term(f => f.Title, keywords, 2.0)
                        //        ||
                        //        q.Term(f => f.Description, keywords, 1.0);

                        return query;
                    });    
                }
                
                return s;
            });

            var responses = search.Documents.ToList();
            responses
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
