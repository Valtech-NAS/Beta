namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Nest;
    using NLog;

    public class TraineeshipsSearchProvider : IVacancySearchProvider<TraineeshipSummaryResponse, TraineeshipSearchParameters>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _vacancySearchMapper;

        public TraineeshipsSearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, IMapper vacancySearchMapper)
        {
            _vacancySearchMapper = vacancySearchMapper;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public SearchResults<TraineeshipSummaryResponse> FindVacancies(TraineeshipSearchParameters parameters)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof (TraineeshipSummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(TraineeshipSummary));

            Logger.Debug("Calling legacy vacancy search for DocumentNameForType={0} on IndexName={1}", documentTypeName, indexName);

            var search = PerformSearch(parameters, client, indexName, documentTypeName);
            var responses = _vacancySearchMapper.Map<IEnumerable<TraineeshipSummary>, IEnumerable<TraineeshipSummaryResponse>>(search.Documents).ToList();

            responses.ForEach(r =>
            {
                var hitMd = search.HitsMetaData.Hits.First(h => h.Id == r.Id.ToString(CultureInfo.InvariantCulture));

                if (parameters.Location != null)
                {
                    if (parameters.SortType == VacancySortType.ClosingDate ||
                        parameters.SortType == VacancySortType.Distance)
                    {
                        r.Distance = double.Parse(hitMd.Sorts.Skip(hitMd.Sorts.Count() - 1).First().ToString());
                    }
                }

                r.Score = hitMd.Score;
            });

            Logger.Debug("{0} search results returned", search.Total);

            var results = new SearchResults<TraineeshipSummaryResponse>(search.Total, parameters.PageNumber, responses, null);

            return results;
        }

        public SearchResults<TraineeshipSummaryResponse> FindVacancy(string vacancyReference)
        {
            throw new NotImplementedException();
        }

        private ISearchResponse<TraineeshipSummary> PerformSearch(TraineeshipSearchParameters parameters, ElasticClient client, string indexName,
            string documentTypeName)
        {
            var search = client.Search<TraineeshipSummary>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);
                s.Skip((parameters.PageNumber - 1)*parameters.PageSize);
                s.Take(parameters.PageSize);

                s.TrackScores();

                switch (parameters.SortType)
                {
                    case VacancySortType.Distance:
                        s.SortGeoDistance(g =>
                        {
                            g.PinTo(parameters.Location.GeoPoint.Latitude, parameters.Location.GeoPoint.Longitude)
                                .Unit(GeoUnit.Miles).OnField(f => f.Location);
                            return g;
                        });
                        break;
                    case VacancySortType.ClosingDate:
                        s.Sort(v => v.OnField(f => f.ClosingDate).Ascending());
                        if (parameters.Location == null) 
                        {
                            break;
                        }
                        //Need this to get the distance from the sort.
                        //Was trying to get distance in relevancy without this sort but can't .. yet
                        s.SortGeoDistance(g =>
                        {
                            g.PinTo(parameters.Location.GeoPoint.Latitude, parameters.Location.GeoPoint.Longitude)
                                .Unit(GeoUnit.Miles).OnField(f => f.Location);
                            return g;
                        });
                        break;
                }

                if (parameters.Location != null)
                {
                    s.Filter(f => f
                        .GeoDistance(vs => vs
                            .Location, descriptor => descriptor
                                .Location(parameters.Location.GeoPoint.Latitude, parameters.Location.GeoPoint.Longitude)
                                .Distance(parameters.SearchRadius, GeoUnit.Miles)));
                }

                return s;
            });
            return search;
        }
    }
}