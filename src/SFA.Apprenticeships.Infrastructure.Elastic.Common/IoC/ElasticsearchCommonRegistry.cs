namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC
{
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using StructureMap.Configuration.DSL;

    public class ElasticsearchCommonRegistry : Registry
    {
        public ElasticsearchCommonRegistry()
        {
            For<ElasticsearchConfiguration>().Singleton().Use(ElasticsearchConfiguration.Instance);
            For<IElasticsearchClientFactory>().Singleton().Use<ElasticsearchClientFactory>();
        }
    }
}
