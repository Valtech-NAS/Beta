
namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces
{
    public interface ISpecification<in T>
    {
        string Build(T entity);
    }
}
