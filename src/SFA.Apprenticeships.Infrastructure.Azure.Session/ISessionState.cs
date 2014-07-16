namespace SFA.Apprenticeships.Infrastructure.Azure.Session
{
    // TODO: MOVE: need to move this as it should be somewhere more abstract rather than in an azure specific project
    public interface ISessionState
    {
        ISessionState Clear();
        ISessionState Delete(string key);
        object Get(string key);
        T Get<T>(string key) where T : class;
        ISessionState Store(string key, object value);
    }
}
