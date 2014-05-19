namespace SFA.Apprenticeships.Common.Interfaces.Elasticsearch
{
    public interface ISortableSpecification<in T> : ISpecification<T>
    {
        int SortOrder { get; set; }
    }
}
