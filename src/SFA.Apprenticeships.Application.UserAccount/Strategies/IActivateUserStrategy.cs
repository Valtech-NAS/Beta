namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;

    public interface IActivateUserStrategy
    {
        void Activate(string username, string activationCode);
    }
}
