namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using FluentAssertions;
    using Nest;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services;
    using StructureMap;

    [TestFixture]
    public class IndexingInitialisationTests
    {
        private ElasticClient _elasticClient;
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration = ElasticsearchConfiguration.Instance;

        [SetUp]
        public void SetUp()
        {
            var settings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _elasticClient = new ElasticClient(settings);
        }

        [Test]
        public void ShouldCreateIndexAndMapping()
        {
            foreach (var index in _elasticsearchConfiguration.Indexes)
            {
                _elasticClient.IndexExists(index.Name).Exists.Should().BeFalse();
            }
            
            var vis = ObjectFactory.GetInstance<IVacancyIndexerService>();

            foreach (var index in _elasticsearchConfiguration.Indexes)
            {
                _elasticClient.IndexExists(index.Name).Exists.Should().BeTrue();
                var mapping = _elasticClient.GetMapping(index.MappingType, index.Name);
                mapping.Should().NotBeNull();
            }
        }
    }
}
