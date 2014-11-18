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

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            _elasticsearchClientFactory = ObjectFactory.GetInstance<IElasticsearchClientFactory>();
#pragma warning restore 0618

            _vacancyIndexAlias = _elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary));
        }

        [Test, Category("Integration")]
        public void ShouldCreateScheduledIndexAndMapping()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = string.Format("{0}.{1}", _vacancyIndexAlias, scheduledDate.ToString("yyyy-MM-dd-HH"));

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();
#pragma warning restore 0618

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<VacancySummary>(i => i.Index(indexName));
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

        [Test, Category("Integration"), Ignore("Is deleting index from environments, consider using different index name")]
        public void ShouldCreateScheduledIndexAndPublishWithAlias()
        {
            var scheduledDate = DateTime.Now; //new DateTime(2000, 1, 1);
            var indexName = string.Format("{0}.{1}", _vacancyIndexAlias, scheduledDate.ToString("yyyy-MM-dd-HH"));

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();
#pragma warning restore 0618

            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            vis.SwapIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(_vacancyIndexAlias)).Exists.Should().BeTrue();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(_vacancyIndexAlias)).Exists.Should().BeFalse();
        }

        [Test, Category("Integration"), Category("SmokeTests"), Ignore("Is deleting index from environments, consider using different index name")]
        public void ShouldCreateIndexAndIndexDocument()
        {
            var indexName = _vacancyIndexAlias + ".2000-01-01-00";
            var scheduledDate = new DateTime(2000, 1, 1);
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();
#pragma warning restore 0618

            //DeleteIndexIfExists(indexName);
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
                Location = new Domain.Entities.Locations.GeoPoint() { Latitude = 1d, Longitude = 2d },
                VacancyLocationType = VacancyLocationType.NonNational,
                ScheduledRefreshDateTime = new DateTime(2000, 1, 1)
            };

            vis.Index(vacancySummary);
            
            _elasticClient.DeleteIndex(i => i.Index(indexName));
        }
    }
}
