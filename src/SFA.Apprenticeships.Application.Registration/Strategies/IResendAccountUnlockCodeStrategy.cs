namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface IResendAccountUnlockCodeStrategy
    {
        void ResendAccountUnlockCode(string username);
    }
}
