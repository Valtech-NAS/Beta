namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;
    using Domain.Entities.Users;

    /// <summary>
    /// For self-service users to register, activate and manage their credentials. 
    /// Role agnostic (i.e. is not aware of specific roles such as candidate).
    /// Uses the user repository
    /// </summary>
    public interface IUserAccountService
    {
        bool IsUsernameAvailable(string username); // true if matches an existing *activated* account

        void Register(string username, Guid userId, string activationCode, UserRoles roles);

        void Activate(string username, string activationCode);

        void ResendActivationCode(string username);

        void SendPasswordResetCode(string username);

        void ResetForgottenPassword(string username, string passwordCode, string newPassword);

        void ResendAccountUnlockCode(string username);

        void UnlockAccount(string username, string accountUnlockCode);

        User GetUser(string username);

        string[] GetRoleNames(User user);
    }
}
