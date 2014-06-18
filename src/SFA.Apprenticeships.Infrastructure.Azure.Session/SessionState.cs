namespace SFA.Apprenticeships.Infrastructure.Azure.Session
{
    using System.Web;

    public class AzureSessionState : ISessionState
    {
        private readonly HttpSessionStateBase _session;

        public AzureSessionState(HttpSessionStateBase session)
        {
            _session = session;
        }

        public ISessionState Clear()
        {
            //_logger.Debug("Clearing session");
            _session.RemoveAll();
            return this;
        }

        public ISessionState Delete(string key)
        {
            //_logger.Debug("Deleting '{0}' from session", key);
            _session.Remove(key);
            return this;
        }

        public object Get(string key)
        {
            //_logger.Debug("Retrieving '{0}' from session", key);
            return _session[key];
        }

        public T Get<T>(string key) where T : class
        {
            //_logger.Debug("Retrieving '{0}' from session", key);
            return _session[key] as T;
        }

        public ISessionState Store(string key, object value)
        {
            //_logger.Debug("Storing '{0}' in session", key);
            _session[key] = value;
            return this;
        }
    }
}
