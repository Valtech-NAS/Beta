namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;

    public interface ISessionStateProvider
    {
        ISessionStateProvider Clear();
        ISessionStateProvider Delete(string key);
        object Get(string key);
        T Get<T>(string key) where T : class;
        ISessionStateProvider Store(string key, object value);
    }
}
