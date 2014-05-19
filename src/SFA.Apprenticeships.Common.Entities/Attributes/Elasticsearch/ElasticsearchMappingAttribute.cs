using System;

namespace SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch
{
    public class ElasticsearchMappingAttribute : Attribute
    {
        public string Name { get; set; }
        public string Index { get; set; }
    }
}
