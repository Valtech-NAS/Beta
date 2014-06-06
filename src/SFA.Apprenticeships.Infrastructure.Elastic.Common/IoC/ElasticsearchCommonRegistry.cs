namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC
{
    using Configuration;
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
