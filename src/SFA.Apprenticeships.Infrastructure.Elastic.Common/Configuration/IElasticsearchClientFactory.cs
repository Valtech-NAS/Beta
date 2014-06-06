namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using Nest;

    public interface IElasticsearchClientFactory
    {
        ElasticClient GetElasticClient();

        string GetIndexNameForType(Type attributeMappedType);
    }
}
