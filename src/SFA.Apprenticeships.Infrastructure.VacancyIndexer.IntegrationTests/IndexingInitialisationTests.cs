namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using System;
    using FluentAssertions;
    using Nest;
    using NUnit.Framework;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Services;
    using StructureMap;

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

            _elasticsearchClientFactory = ObjectFactory.GetInstance<IElasticsearchClientFactory>();
            _vacancyIndexAlias = _elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary));
        }

        [Test]
        public void ShouldCreateScheduledIndexAndMapping()
        {
            var indexName = _vacancyIndexAlias + ".2000-01-01";
            var scheduledDate = new DateTime(2000, 1, 1);
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();

            _elasticClient.IndexExists(indexName).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(indexName).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping(typeof(VacancySummary), indexName);
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(indexName);
            _elasticClient.IndexExists(indexName).Exists.Should().BeFalse();
        }

        [Test]
        public void ShouldCreateScheduledIndexAndPublishWithAlias()
        {
            var indexName = _vacancyIndexAlias + ".2000-01-01";
            var scheduledDate = new DateTime(2000, 1, 1);
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();

            _elasticClient.IndexExists(indexName).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            vis.SwapIndex(scheduledDate);
            _elasticClient.IndexExists(_vacancyIndexAlias).Exists.Should().BeTrue();

            _elasticClient.DeleteIndex(indexName);
            _elasticClient.IndexExists(_vacancyIndexAlias).Exists.Should().BeFalse();
        }
    }
}
