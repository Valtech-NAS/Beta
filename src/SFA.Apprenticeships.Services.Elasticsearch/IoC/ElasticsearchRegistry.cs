using SFA.Apprenticeships.Common.Configuration;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.Elasticsearch.Service;
using StructureMap.Configuration.DSL;

namespace SFA.Apprenticeships.Services.Elasticsearch.IoC
{
    public class ElasticsearchRegistry : Registry
    {
        public ElasticsearchRegistry()
        {
            For<IElasticSearchService>().Use<ElasticsearchService>().Ctor<IConfigurationManager>();
        }
    }
}