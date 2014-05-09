namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
{
    public interface ISortableSpecification<in T> : ISpecification<T>
    {
        int SortOrder { get; set; }
    }
}
