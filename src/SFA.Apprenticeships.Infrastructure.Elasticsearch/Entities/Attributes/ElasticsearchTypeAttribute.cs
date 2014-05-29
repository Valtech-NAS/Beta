namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Attributes
{
    using System;

    public class ElasticsearchTypeAttribute : Attribute
    {
        public ElasticsearchTypeAttribute()
        {
        }

        public ElasticsearchTypeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Format { get; set; }
    }
}
