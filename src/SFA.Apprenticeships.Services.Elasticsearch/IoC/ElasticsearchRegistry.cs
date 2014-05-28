namespace SFA.Apprenticeships.Services.Elasticsearch.IoC
{
    using SFA.Apprenticeships.Services.Elasticsearch.Service;
    using StructureMap.Configuration.DSL;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using SFA.Apprenticeships.Services.Elasticsearch.Interfaces;

    public class ElasticsearchRegistry : Registry
    {
        public ElasticsearchRegistry()
        {
            For<IElasticsearchService>().Use<ElasticsearchService>().Ctor<IConfigurationManager>();
        }
    }
}