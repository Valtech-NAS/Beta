namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    /// <summary>
    /// For self-service users to register, activate and manage their credentials. 
    /// Uses the user repository
    /// </summary>
    public interface IRegistrationService
    {
        bool IsUsernameAvailable(string username); // true if matches an existing *activated* account

        void Register(string username, Guid userId, string activationCode);

        void SendActivationCode(string username); // resend an activation code

        void Activate(string username, string activationCode);

        void SendPasswordCode(string username); // resend a password reset code

        void ChangeForgottenPassword(string username, string passwordCode, string newPassword);
    }
}
