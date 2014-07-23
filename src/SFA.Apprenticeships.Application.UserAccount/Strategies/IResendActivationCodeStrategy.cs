namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IResendActivationCodeStrategy
    {
        void ResendActivationCode(string username);
    }
}
