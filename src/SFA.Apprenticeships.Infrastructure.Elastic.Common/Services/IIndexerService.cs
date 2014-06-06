namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Services
{
    public interface IIndexerService<in TSource>
    {
        void Index(TSource objectToIndex);
    }
}
