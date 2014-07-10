namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;
    using Domain.Entities.Users;

    /// <summary>
    /// For self-service users to register, activate and manage their credentials. 
    /// Role agnostic (i.e. is not aware of specific roles such as candidate).
    /// Uses the user repository
    /// </summary>
    public interface IRegistrationService
    {
        bool IsUsernameAvailable(string username); // true if matches an existing *activated* account

        void Register(string username, Guid userId, string activationCode, UserRoles roles);

        void Activate(string username, string activationCode);

        void ResendActivationCode(string username); // resend an activation code - todo: may remove

        void ResendPasswordCode(string username); // resend a password reset code - todo: may remove

        void ChangeForgottenPassword(string username, string passwordCode, string newPassword);
    }
}
             