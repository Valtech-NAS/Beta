namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    public interface IIndexerService<in TSource>
    {
        void Index(TSource objectToIndex);
    }
}
