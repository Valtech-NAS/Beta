using System.ComponentModel;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    public enum SortDirectionType
    {
        [Description("asc")]
        Ascending,

        [Description("desc")]
        Descending,
    }
}
