namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface IResetForgottenPasswordStrategy
    {
        void ResetForgottenPassword(string username, string passwordCode, string newPassword);
    }
}
