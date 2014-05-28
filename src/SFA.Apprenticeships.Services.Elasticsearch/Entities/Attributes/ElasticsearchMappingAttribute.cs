namespace SFA.Apprenticeships.Services.Elasticsearch.Entities.Attributes
{
    using System;

    public class ElasticsearchMappingAttribute : Attribute
    {
        public string Document { get; set; }
        public string Index { get; set; }
    }
}
