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
    using VacancyLocationType = Domain.Entities.Vacancies.VacancyLocationType;

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

        [Test, Category("Integration")]
        public void ShouldCreateScheduledIndexAndMapping()
        {
            var scheduledDate = DateTime.Now; //new DateTime(2000, 1, 1);
            var indexName = string.Format("{0}.{1}", _vacancyIndexAlias, scheduledDate.ToString("yyyy-MM-dd-HH-mm"));
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();

            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<VacancySummary>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        [Test, Category("Integration")]
        public void ShouldCreateScheduledIndexAndPublishWithAlias()
        {
            var scheduledDate = DateTime.Now; //new DateTime(2000, 1, 1);
            var indexName = string.Format("{0}.{1}", _vacancyIndexAlias, scheduledDate.ToString("yyyy-MM-dd-HH-mm"));
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();

            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            vis.SwapIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(_vacancyIndexAlias)).Exists.Should().BeTrue();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(_vacancyIndexAlias)).Exists.Should().BeFalse();
        }

        [Test, Category("Integration")]
        public void ShouldCreateIndexAndIndexDocument()
        {
            var indexName = _vacancyIndexAlias + ".2000-01-01";
            var scheduledDate = new DateTime(2000, 1, 1);
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();

            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            vis.SwapIndex(scheduledDate);

            var vacancySummary = new VacancySummaryUpdate
            {
                Id = 1,
                Title = "Title test",
                Description = "Description test",
                EmployerName = "Employer name test",
                ClosingDate = DateTime.Today,
                Location = new Domain.Entities.Locations.GeoPoint() {Latitude = 1d, Longitude= 2d},
                VacancyLocationType = VacancyLocationType.NonNational,
                ScheduledRefreshDateTime = new DateTime(2000, 1, 1)
            };

            vis.Index(vacancySummary);

            _elasticClient.DeleteIndex(i => i.Index(indexName));
        }
    }
}
