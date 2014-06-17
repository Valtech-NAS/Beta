namespace SFA.Apprenticeships.Infrastructure.Azure.Session
{
    public interface ISessionState
    {
        ISessionState Clear();
        ISessionState Delete(string key);
        object Get(string key);
        T Get<T>(string key) where T : class;
        ISessionState Store(string key, object value);
    }
}