namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface IUnlockAccountStrategy
    {
        void UnlockAccount(string username, string accountUnlockCode);
    }
}
