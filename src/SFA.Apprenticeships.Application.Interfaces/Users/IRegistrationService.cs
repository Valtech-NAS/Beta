namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    /// <summary>
    /// For self-service users to register, activate and manage their credentials. 
    /// Uses the user repository
    /// </summary>
    public interface IRegistrationService
    {
        bool IsUsernameAvailable(string username);

        void Register(string username, Guid userId, string activationCode);

        void SendActivationCode(string username);

        void ActivateUser(string username, string activationCode);

        void SendPasswordCode(string username);

        void ChangeForgottenPassword(string username, string passwordCode, string newPassword);
    }
}
