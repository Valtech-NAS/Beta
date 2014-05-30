namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.IoC
{
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Service;
    using StructureMap.Configuration.DSL;

    public class ElasticsearchRegistry : Registry
    {
        public ElasticsearchRegistry()
        {
            For<IElasticsearchService>().Use<ElasticsearchService>();
        }
    }
}
