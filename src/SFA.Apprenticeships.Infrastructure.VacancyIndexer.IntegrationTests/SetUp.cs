namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using Nest;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC;
    using StructureMap;

    [SetUpFixture]
    public class SetUpFixture
    {
        private ElasticClient _elasticClient;
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration = ElasticsearchConfiguration.Instance;

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
                if (elasticsearchIndexConfiguration.Name.EndsWith("_integration_test"))
                {
                    _elasticClient.DeleteIndex(i => i.Index(elasticsearchIndexConfiguration.Name));
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (IElasticsearchIndexConfiguration elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                if (elasticsearchIndexConfiguration.Name.EndsWith("_integration_test"))
                {
                    _elasticClient.DeleteIndex(i => i.Index(elasticsearchIndexConfiguration.Name));
                }
            }
        }
    }
}
