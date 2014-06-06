namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using Nest;

    public interface IElasticsearchClientFactory
    {
        ElasticClient GetElasticClient();
    }
}
