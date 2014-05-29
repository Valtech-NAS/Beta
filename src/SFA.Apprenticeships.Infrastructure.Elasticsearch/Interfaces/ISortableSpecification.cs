namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces
{
    public interface ISortableSpecification<in T> : ISpecification<T>
    {
        int SortOrder { get; set; }
    }
}
