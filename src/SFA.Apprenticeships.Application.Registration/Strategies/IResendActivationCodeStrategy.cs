namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface IResendActivationCodeStrategy
    {
        void ResendActivationCode(string username);
    }
}
