namespace SFA.Apprenticeships.Service.Vacancy
{
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.Entities;
    using Infrastructure.Elastic.Common.IoC;
    using Nest;
    using StructureMap;
    using Types;

    public class SearchProvider
    {
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly string _documentTypeName;

        public SearchProvider()
        {
            ObjectFactory.Configure(c =>
            {
                c.AddRegistry<CommonRegistry>();
                c.AddRegistry<ElasticsearchCommonRegistry>();
            });

            var elasticsearchClientFactory = ObjectFactory.GetInstance<IElasticsearchClientFactory>();
            _client = elasticsearchClientFactory.GetElasticClient();
            _indexName = elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary));
            _documentTypeName = elasticsearchClientFactory.GetDocumentNameForType(typeof(VacancySummary));
        }

        public SearchResults<VacancySummaryResponse> Search(Types.SearchRequest request)
        {
            var searchRequestExtended = new SearchRequestExtended(request);

            var search = _client.Search<VacancySummaryResponse>(s =>
            {
                s.Index(_indexName);
                s.Type(_documentTypeName);
                s.Take(1000);

                s.Query(q =>
                {
                    QueryContainer query = null;

                    if (searchRequestExtended.UseJobTitleTerms && !string.IsNullOrWhiteSpace(searchRequestExtended.JobTitleTerms))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.Title).Query(searchRequestExtended.JobTitleTerms);
                            BuildFieldQuery(m, searchRequestExtended.JobTitleFactors);
                        });

                        query = BuildContainer(null, queryClause);
                    }
                    else
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.Title).Query(searchRequestExtended.KeywordTerms);
                            BuildFieldQuery(m, searchRequestExtended.JobTitleFactors);
                        });

                        query = BuildContainer(null, queryClause);
                    }

                    if (searchRequestExtended.SearchDescriptionField && !string.IsNullOrWhiteSpace(searchRequestExtended.KeywordTerms))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.Description).Query(searchRequestExtended.KeywordTerms);
                            BuildFieldQuery(m, searchRequestExtended.DescriptionFactors);
                        });
                        query = BuildContainer(query, queryClause);
                    }

                    if (searchRequestExtended.SearchEmployerNameField && !string.IsNullOrWhiteSpace(searchRequestExtended.KeywordTerms))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.EmployerName).Query(searchRequestExtended.KeywordTerms);
                            BuildFieldQuery(m, searchRequestExtended.EmployerFactors);
                        });
                        query = BuildContainer(query, queryClause);
                    }

                    return query;
                });

                return s;
            });

            var results = search.Documents.ToList();
            results.ForEach(r => r.Score = search.HitsMetaData.Hits.First(h => h.Id == r.Id.ToString(CultureInfo.InvariantCulture)).Score);
            var searchResults = new SearchResults<VacancySummaryResponse>(search.Total, 1, results);

            return searchResults;
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

        private void BuildFieldQuery(MatchQueryDescriptor<VacancySummaryResponse> queryDescriptor, KeywordFactors searchFactors)
        {
            if (searchFactors.Boost.HasValue)
            {
                queryDescriptor.Boost(searchFactors.Boost.Value);
            }

            if (searchFactors.Fuzziness.HasValue)
            {
                queryDescriptor.Fuzziness(searchFactors.Fuzziness.Value);
            }

            if (searchFactors.FuzzinessPrefix.HasValue)
            {
                queryDescriptor.PrefixLength(searchFactors.FuzzinessPrefix.Value);
            }

            if (searchFactors.MatchAllKeywords)
            {
                queryDescriptor.Operator(Operator.And);
            }

            if (searchFactors.MinimumMatch.HasValue)
            {
                queryDescriptor.MinimumShouldMatch(searchFactors.MinimumMatch.Value + "%");
            }

            if (searchFactors.PhraseProximity.HasValue)
            {
                queryDescriptor.Slop(searchFactors.PhraseProximity.Value);
            }   
        }
    }
}
