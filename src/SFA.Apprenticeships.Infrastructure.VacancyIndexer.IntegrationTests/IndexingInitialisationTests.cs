namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using FluentAssertions;
    using Nest;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC;
    using StructureMap;

    [TestFixture]
    public class IndexingInitialisationTests
    {
        private ElasticClient _elasticClient;
        private ElasticsearchConfiguration _elasticsearchConfiguration = ElasticsearchConfiguration.Instance;

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
            });

            var settings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _elasticClient = new ElasticClient(settings);

            foreach (IElasticsearchIndexConfiguration elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                _elasticClient.DeleteIndex(elasticsearchIndexConfiguration.Name);
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (IElasticsearchIndexConfiguration elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                _elasticClient.DeleteIndex(elasticsearchIndexConfiguration.Name);
            }            
        }

        [Test]
        public void ShouldCreateIndexAndMapping()
        {
            foreach (var index in _elasticsearchConfiguration.Indexes)
            {
                _elasticClient.IndexExists(index.Name).Exists.Should().BeFalse();
            }
            
            var vis = ObjectFactory.GetInstance<IIndexerService<VacancySummaryUpdate>>();

            foreach (var index in _elasticsearchConfiguration.Indexes)
            {
                _elasticClient.IndexExists(index.Name).Exists.Should().BeTrue();
                var mapping = _elasticClient.GetMapping(index.MappingType, index.Name);
                mapping.Should().NotBeNull();
            }
        }
    }
}
