namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    public interface IIndexingService<in TSource>
    {
        void Index(string id, TSource objectToIndex);
    }
}
