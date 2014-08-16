namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;

    public interface IUserDataProvider //todo: refactor!
    {
        UserContext GetUserContext();

        void SetUserContext(string userName, string fullName);

        void Clear();

        void Push(string key, string value);

        string Get(string key);

        string Pop(string key);
    }
}
