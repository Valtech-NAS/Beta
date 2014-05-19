using System.ComponentModel;

namespace SFA.Apprenticeships.Common.Interfaces.Enums.Elasticsearch
{
    public enum ElasticsearchIndexType
    {
        [Description("analyzed")] 
        Analyzed = 0,

        [Description("not_analyzed")] 
        NotAnalyzed,

        [Description("no")] 
        NotIndexed,
    }
}