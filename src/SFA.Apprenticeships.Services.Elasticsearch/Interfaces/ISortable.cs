namespace SFA.Apprenticeships.Services.Elasticsearch.Interfaces
{
    using SFA.Apprenticeships.Services.Elasticsearch.Entities.Elasticsearch;

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
