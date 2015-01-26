namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using Elastic.Common.Configuration;
    using Elastic.Common.IoC;
    using IoC;
    using Nest;
    using NUnit.Framework;
    using StructureMap;

    [SetUpFixture]
    public class SetUpFixture
    {
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration = ElasticsearchConfiguration.Instance;
        private ElasticClient _elasticClient;

        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
            });
#pragma warning restore 0618

            var settings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _elasticClient = new ElasticClient(settings);

            foreach (
                IElasticsearchIndexConfiguration elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
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
            foreach (
                IElasticsearchIndexConfiguration elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                if (elasticsearchIndexConfiguration.Name.EndsWith("_integration_test"))
                {
                    _elasticClient.DeleteIndex(i => i.Index(elasticsearchIndexConfiguration.Name));
                }
            }
        }
    }
}