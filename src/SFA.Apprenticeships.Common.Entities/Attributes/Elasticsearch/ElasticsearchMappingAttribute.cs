using System;

namespace SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch
{
    public class ElasticsearchMappingAttribute : Attribute
    {
        public string Document { get; set; }
        public string Index { get; set; }
    }
}
