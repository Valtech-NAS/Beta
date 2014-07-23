namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IResetForgottenPasswordStrategy
    {
        void ResetForgottenPassword(string username, string passwordCode, string newPassword);
    }
}
