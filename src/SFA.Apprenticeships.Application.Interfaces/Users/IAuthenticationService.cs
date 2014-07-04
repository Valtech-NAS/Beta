namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    /// <summary>
    /// For users to authentication against the directory
    /// </summary>
    public interface IAuthenticationService
    {
        void AuthenticateUser(Guid id, string password);

        void CreateUser(Guid id, string password);

        void ChangePassword(Guid id, string newPassword);
    }
}
