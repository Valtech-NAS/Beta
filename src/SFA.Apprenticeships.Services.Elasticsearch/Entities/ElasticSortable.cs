using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public class ElasticSortable<T> : ISortable<T>
    {
        public bool HasValue { get; set; }
        public T Value { get; set; }
        public bool SortEnabled { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}
