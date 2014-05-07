using System.ComponentModel;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public enum SortDirectionType
    {
        [Description("asc")]
        Ascending,

        [Description("desc")]
        Descending,
    }
}
