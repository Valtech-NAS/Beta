namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    public interface IIndexingService<in T>
    {
        void Index(string id, T objectToIndex);
    }
}
