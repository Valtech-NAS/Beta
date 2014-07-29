namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface ISendAccountUnlockCodeStrategy
    {
        void SendAccountUnlockCode(string username);
    }
}
