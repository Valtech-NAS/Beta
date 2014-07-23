namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services
{
    using System;
    using Nest;
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using NLog;

    public class VacancyIndexerService : IVacancyIndexerService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public VacancyIndexerService(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public void Index(VacancySummaryUpdate vacancySummaryToIndex)
        {
            Logger.Debug("Indexing vacancy item");
            
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, vacancySummaryToIndex.ScheduledRefreshDateTime);
            var vacancySummaryElastic = _mapper.Map<VacancySummaryUpdate, VacancySummary>(vacancySummaryToIndex);

            Logger.Debug("Indexing vacancy item to index: {0}", newIndexName);

            var client = _elasticsearchClientFactory.GetElasticClient();
            client.Index(vacancySummaryElastic, newIndexName);

            Logger.Debug("Indexed vacancy item");
        }

        public void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            Logger.Debug("Creating new vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();

            var indexExistsResponse = client.IndexExists(newIndexName);
            if (!indexExistsResponse.Exists)
            {
                var indexSettings = new IndexSettings();
                client.CreateIndex(newIndexName, indexSettings);
                client.MapFromAttributes(typeof (VacancySummary), newIndexName);

                Logger.Debug("Created new vacancy search index named: {0}", newIndexName);
            }
            else
            {
                Logger.Error("Vacancy search index already exists: {0}", newIndexName);
                // TODO: EXCEPTION: specialise the exception type thrown here and log - should prevent the indexing job from continuing.
                throw new Exception(string.Format("Index already created: {0}", newIndexName));
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
            client.Swap(indexAlias, existingIndexesOnAlias, new[] {newIndexName});

            Logger.Debug("Swapped vacancy search index alias to new index: {0}", newIndexName);
        }

        private string GetIndexAlias()
        {
            return _elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary));
        }

        private string GetIndexNameAndDateExtension(string indexAlias, DateTime dateTime)
        {
            return string.Format("{0}.{1}", indexAlias, dateTime.ToString("yyyy-MM-dd"));
        }
    }
}
