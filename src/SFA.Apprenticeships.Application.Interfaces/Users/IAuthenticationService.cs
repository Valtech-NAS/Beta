namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    /// <summary>
    /// To authenticate users against the directory and manage user directory accounts
    /// </summary>
    public interface IAuthenticationService
    {
        void AuthenticateUser(Guid id, string password);

        void CreateUser(Guid id, string password);

        void ChangePassword(Guid id, string newPassword);
    }
}
