namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Attributes
{
    using System;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Elasticsearch;

    public class ElasticsearchIndexAttribute : Attribute
    {
        public string Name { get; set; }
        public ElasticsearchIndexType Index { get; set; }
    }
}
