namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IResendAccountUnlockCodeStrategy
    {
        void ResendAccountUnlockCode(string username);
    }
}
