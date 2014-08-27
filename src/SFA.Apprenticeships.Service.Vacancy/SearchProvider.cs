namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.Linq;
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

        public VacancySummaryResponse[] Search(SearchRequest request)
        {
            //todo: 
            // 1. map request parameter values to search request
            //    will be passed as string... map to enum and parse values (bool, double, etc.)
            //    if any fail, return "bad request" fault with name of failed parameters
            //
            // 2. invoke search component (using NEST) passing in the parsed parameters and values
            //    map search results to DTOs (incl. score)
            //
            // 3. return DTOs along with original request (for correlation in test tool)

            var searchExtended = new SearchRequestExtended(request);
            return SearchExtended(searchExtended);

            //var results = Enumerable.Range(1, 10).Select(i => new VacancySummaryResponse
            //{
            //    Id = i,
            //    Title = "Title #" + i,
            //    Description = "Vacancy description #" + i,
            //    EmployerName = "Employer name #" + i,
            //    ClosingDate = DateTime.UtcNow.AddDays(i),
            //    Score = 1.0
            //});

            //return results.ToArray();
        }

        private VacancySummaryResponse[] SearchExtended(SearchRequestExtended searchRequestExtended)
        {
            var search = _client.Search<VacancySummaryResponse>(s =>
            {
                s.Index(_indexName);
                s.Type(_documentTypeName);
                s.Take(1000);

                if (searchRequestExtended.UseJobTitleTerms && !string.IsNullOrWhiteSpace(searchRequestExtended.JobTitleTerms))
                {
                    //if (searchRequestExtended.JobTitleFactors.MatchAllKeywords)
                    //{

                    //}
                    //else
                    //{
                        //s.Query(q =>
                        //    q.Bool(fz => fz
                        //        .Should(s => s
                        //            .Match(mp => mp
                        //                .OnField(f => f.Title)
                        //                .PrefixLength(searchRequestExtended.JobTitleFactors.FuzzinessPrefix)
                        //                .Boost(searchRequestExtended.JobTitleFactors.Boost)
                        //                .QueryString(searchRequestExtended.JobTitleTerms)),
                        //                        descriptor => descriptor 
                        //                 .);

                        s.Query(q =>
                            q.Fuzzy(fz => fz
                                .OnField(f => f.Title)
                                .PrefixLength(searchRequestExtended.JobTitleFactors.FuzzinessPrefix)
                                .Boost(searchRequestExtended.JobTitleFactors.Boost)
                                .Value(searchRequestExtended.JobTitleTerms)));
                    //}
                }

                if (searchRequestExtended.UseJobTitleTerms)
                {
                }

                return s;
            });

            return search.Documents.ToArray();
        }
    }
}
