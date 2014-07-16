namespace SFA.Apprenticeships.Application.Authentication
{
    using System;
    using Interfaces.Users;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;
        public AuthenticationService(IUserDirectoryProvider userDirectoryProvider)
        {
            _userDirectoryProvider = userDirectoryProvider;
        }

        public void AuthenticateUser(Guid id, string password)
        {
            var succeeded = _userDirectoryProvider.AuthenticateUser(id.ToString(), password);

            CheckAndThrowFailureError(succeeded, "User Authentication failed");
        }

        public void CreateUser(Guid id, string password)
        {
            var succeeded = _userDirectoryProvider.CreateUser(id.ToString(), password);

            CheckAndThrowFailureError(succeeded, "User creation failed");
        }

        public void ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            var succeeded = _userDirectoryProvider.ChangePassword(id.ToString(), oldPassword, newPassword);

            CheckAndThrowFailureError(succeeded, "Change password failed");
        }

        private static void CheckAndThrowFailureError(bool succeeded, string errorMessage)
        {
            if (!succeeded)
            {
                throw new Exception(errorMessage); // TODO: EXCEPTION: should use an application exception type
            }
        }
    }
}
