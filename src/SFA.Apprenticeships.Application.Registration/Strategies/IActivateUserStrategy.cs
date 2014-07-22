namespace SFA.Apprenticeships.Application.Registration.Strategies
{
    using System;

    public interface IActivateUserStrategy
    {
        void Activate(string username, string activationCode);
    }
}
