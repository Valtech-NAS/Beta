namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces
{
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Elasticsearch;

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
