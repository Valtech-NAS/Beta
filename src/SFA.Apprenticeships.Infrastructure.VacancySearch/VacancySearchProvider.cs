namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Configuration;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Nest;
    using Newtonsoft.Json.Linq;
    using NLog;

    public class VacancySearchProvider : IVacancySearchProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly SearchConfiguration _searchConfiguration;

        public VacancySearchProvider(IElasticsearchClientFactory elasticsearchClientFactory,
            SearchConfiguration searchConfiguration)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _searchConfiguration = searchConfiguration;
        }

        public SearchResults<VacancySummaryResponse> FindVacancies(SearchParameters parameters)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof (VacancySummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof (VacancySummary));

            Logger.Debug("Calling legacy vacancy search for DocumentNameForType={0} on IndexName={1}", documentTypeName,
                indexName);

            var search = client.Search<VacancySummaryResponse>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);
                s.Skip((parameters.PageNumber - 1)*parameters.PageSize);
                s.Take(parameters.PageSize);

                s.TrackScores();

                s.Query(q =>
                {
                    QueryContainer query = null;

                    if (_searchConfiguration.SearchJobTitleField)
                    {
                        if (_searchConfiguration.UseJobTitleTerms && !string.IsNullOrWhiteSpace(parameters.Keywords))
                        {
                            var queryClause = q.Match(m =>
                            {
                                m.OnField(f => f.Title).Query(parameters.Keywords);
                                BuildFieldQuery(m, _searchConfiguration.SearchTermParameters.JobTitleFactors);
                            });

                            query = BuildContainer(null, queryClause);
                        }
                        else
                        {
                            var queryClause = q.Match(m =>
                            {
                                m.OnField(f => f.Title).Query(parameters.Keywords);
                                BuildFieldQuery(m, _searchConfiguration.SearchTermParameters.JobTitleFactors);
                            });

                            query = BuildContainer(null, queryClause);
                        }
                    }

                    if (_searchConfiguration.SearchDescriptionField && !string.IsNullOrWhiteSpace(parameters.Keywords))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.Description).Query(parameters.Keywords);
                            BuildFieldQuery(m, _searchConfiguration.SearchTermParameters.DescriptionFactors);
                        });
                        query = BuildContainer(query, queryClause);
                    }

                    if (_searchConfiguration.SearchEmployerNameField && !string.IsNullOrWhiteSpace(parameters.Keywords))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.EmployerName).Query(parameters.Keywords);
                            BuildFieldQuery(m, _searchConfiguration.SearchTermParameters.EmployerFactors);
                        });
                        query = BuildContainer(query, queryClause);
                    }

                    var vacancyLocationTypeClause =
                        q.Match(
                            p => p.OnField(f => f.VacancyLocationType).Query(parameters.VacancyLocationType.ToString()));
                    query = BuildContainer(query, vacancyLocationTypeClause);

                    return query;
                });

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
                        //Need this to get the distance from the sort.
                        //Was trying to get distance in relevancy without this sort but can't .. yet
                        s.SortGeoDistance(g =>
                        {
                            g.PinTo(parameters.Location.GeoPoint.Latitude, parameters.Location.GeoPoint.Longitude)
                                .Unit(GeoUnit.Miles).OnField(f => f.Location);
                            return g;
                        });
                        break;
                    case VacancySortType.Relevancy:
                        s.Fields("_source");
                        s.ScriptFields(sf =>
                            sf.Add("distance", sfd => sfd
                                .Params(fp =>
                                {
                                    fp.Add("lat", parameters.Location.GeoPoint.Latitude);
                                    fp.Add("lon", parameters.Location.GeoPoint.Longitude);
                                    return fp;
                                })
                                .Script("doc['location'].arcDistanceInMiles(lat, lon)")));
                        //.Script("doc[\u0027location\u0027].distanceInMiles(lat,lon)")));
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

            var responses = search.Documents.ToList();

            responses.ForEach(r =>
            {
                var hitMd = search.HitsMetaData.Hits.First(h => h.Id == r.Id.ToString(CultureInfo.InvariantCulture));

                if (parameters.SortType == VacancySortType.ClosingDate ||
                    parameters.SortType == VacancySortType.Distance)
                {
                    r.Distance = double.Parse(hitMd.Sorts.Skip(hitMd.Sorts.Count() - 1).First().ToString());
                }
                else
                {
                    //if anyone can find a better way to get this value out, feel free!
                    var array = hitMd.Fields.FieldValues<JArray>("distance");
                    var value = array[0];
                    r.Distance = double.Parse(value.ToString());
                }

                r.Score = hitMd.Score;
            });

            Logger.Debug("{0} search results returned", search.Total);

            var results = new SearchResults<VacancySummaryResponse>(search.Total, parameters.PageNumber, responses);

            return results;
        }

        private QueryContainer BuildContainer(QueryContainer queryContainer, QueryContainer queryClause)
        {
            if (queryContainer == null)
            {
                queryContainer = queryClause;
            }
            else
            {
                queryContainer |= queryClause;
            }

            return queryContainer;
        }

        private static void BuildFieldQuery(MatchQueryDescriptor<VacancySummaryResponse> queryDescriptor,
            ISearchTermFactorsConfiguration searchFactors)
        {
            if (searchFactors.Boost.HasValue)
            {
                queryDescriptor.Boost(searchFactors.Boost.Value);
            }

            if (searchFactors.Fuzziness.HasValue)
            {
                queryDescriptor.Fuzziness(searchFactors.Fuzziness.Value);
            }

            if (searchFactors.FuzzyPrefix.HasValue)
            {
                queryDescriptor.PrefixLength(searchFactors.FuzzyPrefix.Value);
            }

            if (searchFactors.MatchAllKeywords)
            {
                queryDescriptor.Operator(Operator.And);
            }

            if (!string.IsNullOrWhiteSpace(searchFactors.MinimumMatch))
            {
                queryDescriptor.MinimumShouldMatch(searchFactors.MinimumMatch);
            }

            if (searchFactors.PhraseProximity.HasValue)
            {
                queryDescriptor.Slop(searchFactors.PhraseProximity.Value);
            }
        }
    }
}