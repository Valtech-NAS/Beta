
namespace SFA.Apprenticeships.Common.Interfaces.Elasticsearch
{
    public interface ISpecification<in T>
    {
        string Build(T entity);
    }
}
