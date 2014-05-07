namespace SFA.Apprenticeships.Repository.Elasticsearch.Abstract
{
    public interface ISpecification<in T>
    {
        string Build(T entity);
    }
}
