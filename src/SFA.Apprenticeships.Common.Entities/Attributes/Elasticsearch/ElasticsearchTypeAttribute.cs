using System;

namespace SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch
{
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
