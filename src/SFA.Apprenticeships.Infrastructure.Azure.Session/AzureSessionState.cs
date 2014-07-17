namespace SFA.Apprenticeships.Infrastructure.Azure.Session
{
    using System.Web;
    using NLog;

    public class AzureSessionState : ISessionState
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly HttpSessionStateBase _session;

        public AzureSessionState(HttpSessionStateBase session)
        {
            _session = session;
        }

        public ISessionState Clear()
        {
            Logger.Debug("Clearing session");
            _session.RemoveAll();
            return this;
        }

        public ISessionState Delete(string key)
        {
            Logger.Debug("Deleting '{0}' from session", key);
            _session.Remove(key);
            return this;
        }

        public object Get(string key)
        {
            Logger.Debug("Retrieving '{0}' from session", key);
            return _session[key];
        }

        public T Get<T>(string key) where T : class
        {
            Logger.Debug("Retrieving '{0}' from session", key);
            return _session[key] as T;
        }

        public ISessionState Store(string key, object value)
        {
            Logger.Debug("Storing '{0}' in session", key);
            _session[key] = value;
            return this;
        }
    }
}
