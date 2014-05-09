using SFA.Apprenticeships.Services.Elasticsearch.Entities;

namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
{
    public interface ISortable
    {
        bool SortEnabled { get; set; }
        SortDirectionType SortDirection { get; set; }
    }

    public interface ISortable<T> : ISortable
    {
        T Value { get; set; }
    }
}
