namespace SFA.Apprenticeships.Services.Elasticsearch.Entities.Elasticsearch
{
    using System.ComponentModel;

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