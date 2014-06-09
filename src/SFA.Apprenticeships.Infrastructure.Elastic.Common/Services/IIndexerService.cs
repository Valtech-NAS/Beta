namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Services
{
    //todo: move out of common as it's only in 1 service
    public interface IIndexerService<in TSource>
    {
        void Index(TSource objectToIndex);
    }
}
