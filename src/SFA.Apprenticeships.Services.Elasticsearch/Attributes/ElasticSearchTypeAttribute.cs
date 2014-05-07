using System;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Attributes
{
    public class ElasticSearchTypeAttribute : Attribute
    {
        public ElasticSearchTypeAttribute()
        {
        }

        public ElasticSearchTypeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Format { get; set; }
    }
}
