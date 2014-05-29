namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Elasticsearch
{
    using System.ComponentModel;

    public enum SortDirectionType
    {
        [Description("asc")]
        Ascending,

        [Description("desc")]
        Descending,
    }
}
