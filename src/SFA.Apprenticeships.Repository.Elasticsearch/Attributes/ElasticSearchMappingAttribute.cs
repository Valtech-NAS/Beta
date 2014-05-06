using System;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Attributes
{
    public class ElasticSearchMappingAttribute : Attribute
    {
        public ElasticSearchMappingAttribute()
        {
        }

        public ElasticSearchMappingAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
