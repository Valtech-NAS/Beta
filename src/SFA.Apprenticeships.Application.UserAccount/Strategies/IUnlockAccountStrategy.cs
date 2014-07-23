namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IUnlockAccountStrategy
    {
        void UnlockAccount(string username, string accountUnlockCode);
    }
}
