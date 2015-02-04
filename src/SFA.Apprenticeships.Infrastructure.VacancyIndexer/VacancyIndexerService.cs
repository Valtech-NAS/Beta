namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.VacancyEtl.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Nest;
    using VacancyErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class VacancyIndexerService<TSourceSummary, TDestinationSummary> : IVacancyIndexerService<TSourceSummary, TDestinationSummary>
        where TSourceSummary : Domain.Entities.Vacancies.VacancySummary, IVacancyUpdate
        where TDestinationSummary : class, IVacancySummary
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private const int PageSize = 5;
        private const double LondonLatitude = 51;
        private const double LondonLongitude = -0.1;
        private const int SearchRadius = 30;

        public VacancyIndexerService(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper, ILogService logger)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public void Index(TSourceSummary vacancySummaryToIndex)
        {
            _logger.Debug("Indexing vacancy item : {0} ({1})", vacancySummaryToIndex.Title, vacancySummaryToIndex.Id);

            try
            {
                var indexAlias = GetIndexAlias();
                var newIndexName = GetIndexNameAndDateExtension(indexAlias, vacancySummaryToIndex.ScheduledRefreshDateTime);
                var vacancySummaryElastic = _mapper.Map<TSourceSummary, TDestinationSummary>(vacancySummaryToIndex);

                var client = _elasticsearchClientFactory.GetElasticClient();
                client.Index(vacancySummaryElastic, f => f.Index(newIndexName));

                _logger.Debug("Indexed vacancy item : {0} ({1})", vacancySummaryToIndex.Title, vacancySummaryToIndex.Id);
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed indexing traineeship vacancy summary {0}", ex, vacancySummaryToIndex.Id);
            }
        }

        public void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            _logger.Info("Creating new vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();

            var indexExistsResponse = client.IndexExists(i => i.Index(newIndexName));

            if (indexExistsResponse.Exists)
            {
                // If it already exists and is empty, let's delete it and recreate it.
                var totalResults = client.Count<TDestinationSummary>(c =>
                {
                    c.Index(newIndexName);
                    return c;
                });

                if (totalResults.Count == 0)
                {
                    client.DeleteIndex(i => i.Index(newIndexName));
                    indexExistsResponse = client.IndexExists(i => i.Index(newIndexName));
                }
            }

            if (!indexExistsResponse.Exists)
            {
                var indexSettings = new IndexSettings();

                //Standard snowball analyser
                //indexSettings.Analysis.Analyzers.Add("snowball", new SnowballAnalyzer { Language = "English" });

                //Custom snowball analyser without stop words being removed
                var snowballFilter = new SnowballTokenFilter { Language = "English" };
                indexSettings.Analysis.TokenFilters.Add("snowball", snowballFilter);
                indexSettings.Analysis.Analyzers.Add("snowball", new CustomAnalyzer { Tokenizer = "standard", Filter = new[] { "standard", "lowercase", "snowball" } });

                //Matches whole phrases ignoring case
                var keywordLowercaseCustomAnalyzer = new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase" } };
                indexSettings.Analysis.Analyzers.Add("keywordlowercase", keywordLowercaseCustomAnalyzer);

                client.CreateIndex(i => i.Index(newIndexName).InitializeUsing(indexSettings));

                client.Map<TDestinationSummary>(p => p.Index(newIndexName).MapFromAttributes().Properties(prop =>
                    prop.GeoPoint(g => g.Name(n => n.Location))));
                _logger.Info("Created new vacancy search index named: {0}", newIndexName);
            }
            else
            {
                _logger.Error("Vacancy search index already exists: {0}", newIndexName);
                throw new CustomException(string.Format("Index already created: {0}", newIndexName), VacancyErrorCodes.VacancyIndexerServiceError);
            }
        }

        public void SwapIndex(DateTime scheduledRefreshDateTime)
        {
            _logger.Debug("Swapping vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();

            _logger.Debug("Swapping vacancy search index alias to new index: {0}", newIndexName);

            var existingIndexesOnAlias = client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest {Actions = new List<IAliasAction>()};
            
            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });    
            }
            
            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            client.Alias(aliasRequest);

            _logger.Debug("Swapped vacancy search index alias to new index: {0}", newIndexName);
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            _logger.Debug("Checking if the index is correctly created.");

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof (TDestinationSummary));

            var search = client.Search<TDestinationSummary>(s =>
            {
                s.Index(newIndexName);
                s.Type(documentTypeName);
                s.Take(PageSize);

                s.TrackScores();

                s.Filter(f => f
                    .GeoDistance(vs => vs
                        .Location, descriptor => descriptor
                            .Location(LondonLatitude, LondonLongitude)
                            .Distance(SearchRadius, GeoUnit.Miles)));

                return s;
            });

            var result = search.Documents.Any();
            LogResult(result, newIndexName);

            return result;
        }

        private void LogResult(bool result, string newIndexName)
        {
            if (result)
            {
                _logger.Debug("The index {0} is correctly created", newIndexName);
            }
            else
            {
                _logger.Error("The index {0} is not correctly created", newIndexName);
            }
        }

        private string GetIndexAlias()
        {
            return _elasticsearchClientFactory.GetIndexNameForType(typeof(TDestinationSummary));
        }

        private static string GetIndexNameAndDateExtension(string indexAlias, DateTime dateTime)
        {
            return string.Format("{0}.{1}", indexAlias, dateTime.ToString("yyyy-MM-dd-HH"));
        }
    }
}
