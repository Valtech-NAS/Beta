namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services
{
    using System;
    using Nest;
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;

    public class VacancyIndexerService : IVacancyIndexerService
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public VacancyIndexerService(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public void Index(VacancySummaryUpdate vacancySummaryToIndex)
        {
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, vacancySummaryToIndex.ScheduledRefreshDateTime);
            var vacancySummaryElastic = _mapper.Map<VacancySummaryUpdate, VacancySummary>(vacancySummaryToIndex);
            
            var client = _elasticsearchClientFactory.GetElasticClient();
            client.Index(vacancySummaryElastic, newIndexName);
        }

        public void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();

            var indexExistsResponse = client.IndexExists(newIndexName);
            if (!indexExistsResponse.Exists)
            {
                var indexSettings = new IndexSettings();
                client.CreateIndex(newIndexName, indexSettings);
                client.MapFromAttributes(typeof (VacancySummary), newIndexName);
            }
            else
            {
                //TODO: specialise the exception type thrown here and log - should prevent the indexing job from continuing.
                throw new Exception(string.Format("Index already created: {0}", newIndexName));
            }
        }

        public void SwapIndex(DateTime scheduledRefreshDateTime)
        {
            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime);
            var client = _elasticsearchClientFactory.GetElasticClient();
            
            var existingIndexesOnAlias = client.GetIndicesPointingToAlias(indexAlias);
            client.Swap(indexAlias, existingIndexesOnAlias, new[] {newIndexName});
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
