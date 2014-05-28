
namespace SFA.Apprenticeships.Services.Elasticsearch.Interfaces
{
    public interface ISpecification<in T>
    {
        string Build(T entity);
    }
}
