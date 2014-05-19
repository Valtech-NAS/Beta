using System;
using SFA.Apprenticeships.Common.Interfaces.Enums.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch
{
    public class ElasticsearchIndexAttribute : Attribute
    {
        public string Name { get; set; }
        public ElasticsearchIndexType Index { get; set; }
    }
}
