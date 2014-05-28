namespace SFA.Apprenticeships.Services.Elasticsearch.Interfaces
{
    public interface ISortableSpecification<in T> : ISpecification<T>
    {
        int SortOrder { get; set; }
    }
}
