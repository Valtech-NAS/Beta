namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using System;
    using Application.VacancyEtl.Entities;
    using FluentAssertions;
    using Nest;
    using NUnit.Framework;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using StructureMap;
    using VacancyLocationType = Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLocationType;

    [TestFixture]
    public class IndexingInitialisationTests
    {
        private string _vacancyIndexAlias;
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private ElasticClient _elasticClient;
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration = ElasticsearchConfiguration.Instance;

        [SetUp]
        public void SetUp()
        {
            var settings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _elasticClient = new ElasticClient(settings);

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            _elasticsearchClientFactory = ObjectFactory.GetInstance<IElasticsearchClientFactory>();
#pragma warning restore 0618

            _vacancyIndexAlias = _elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary));
        }

        [Test, Category("Integration")]
        public void ShouldCreateScheduledIndexAndMapping()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = string.Format("{0}.{1}", _vacancyIndexAlias, scheduledDate.ToString("yyyy-MM-dd-HH"));

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
#pragma warning restore 0618

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<ApprenticeshipSummary>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(i => i.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(i => i.Index(indexName));
            }
        }
    }
}
