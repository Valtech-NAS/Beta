namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
{
    public interface ISpecification<in T>
    {
        string Build(T entity);
    }
}
