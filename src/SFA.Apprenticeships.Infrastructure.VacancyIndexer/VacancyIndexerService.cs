namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Application.VacancyEtl.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Nest;
    using NLog;
    using ErrorCodes = Domain.Entities.Exceptions.ErrorCodes;

    public class VacancyIndexerService : IVacancyIndexerService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const int PageSize = 5;
        private const double LondonLatitude = 51;
        private const double LondonLongitude = -0.1;
        private const int SearchRadius = 20;

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public VacancyIndexerService(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public void Index(VacancySummaryUpdate vacancySummaryToIndex)
        {
            //Logger.Debug("Indexing vacancy item");
            
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, vacancySummaryToIndex.ScheduledRefreshDateTime);
            var vacancySummaryElastic = _mapper.Map<VacancySummaryUpdate, VacancySummary>(vacancySummaryToIndex);

            //Logger.Debug("Indexing vacancy item to index: {0}", newIndexName);

            var client = _elasticsearchClientFactory.GetElasticClient();
            client.Index(vacancySummaryElastic, f => f.Index(newIndexName));

            //Logger.Debug("Indexed vacancy item");
        }

        public void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Logger.Debug("Creating new vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();

            var indexExistsResponse = client.IndexExists(i => i.Index(newIndexName));

            if (indexExistsResponse.Exists)
            {
                // If it already exists and is empty, let's delete it and recreate it.
                var totalResults = client.Count<VacancySummary>(c =>
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

                client.CreateIndex(i => i.Index(newIndexName).InitializeUsing(indexSettings));
                // client.Map<VacancySummary>(p => p.Index(newIndexName).MapFromAttributes());

                client.Map<VacancySummary>(p => p.Index(newIndexName).MapFromAttributes().Properties(prop =>
                    prop.GeoPoint(g => g.Name(n => n.Location))));
                Logger.Debug("Created new vacancy search index named: {0}", newIndexName);
            }
            else
            {
                Logger.Error("Vacancy search index already exists: {0}", newIndexName);
                throw new CustomException(string.Format("Index already created: {0}", newIndexName), ErrorCodes.VacancyIndexerServiceError);
            }
        }

        public void SwapIndex(DateTime scheduledRefreshDateTime)
        {
            Logger.Debug("Swapping vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();

            Logger.Debug("Swapping vacancy search index alias to new index: {0}", newIndexName);

            var existingIndexesOnAlias = client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest {Actions = new List<IAliasAction>()};
            
            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });    
            }
            
            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            client.Alias(aliasRequest);

            Logger.Debug("Swapped vacancy search index alias to new index: {0}", newIndexName);
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            Logger.Debug("Checking if the index is correctly created.");

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof (VacancySummary));

            var search = client.Search<VacancySummaryResponse>(s =>
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

        private static void LogResult(bool result, string newIndexName)
        {
            var logMessage =
                string.Format(
                    result
                        ? "The index {0} is not correctly created."
                        : "The index {0} is correctly created.",
                    newIndexName);

            Logger.Debug(string.Format("Checked if the index is correctly created. {0}", logMessage));
        }

        private string GetIndexAlias()
        {
            return _elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary));
        }

        private static string GetIndexNameAndDateExtension(string indexAlias, DateTime dateTime)
        {
            return string.Format("{0}.{1}", indexAlias, dateTime.ToString("yyyy-MM-dd-HH"));
        }
    }
}
