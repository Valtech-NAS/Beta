namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    /// <summary>
    /// To authenticate users against the directory and manage user directory accounts
    /// </summary>
    public interface IAuthenticationService
    {
        bool AuthenticateUser(Guid userId, string password);

        void CreateUser(Guid userId, string password);

        void ResetUserPassword(Guid userId, string password);

        void ChangePassword(Guid userId, string oldPassword, string newPassword);
    }
}
