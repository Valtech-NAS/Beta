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

        public bool AuthenticateUser(Guid id, string password)
        {
            return _userDirectoryProvider.AuthenticateUser(id.ToString(), password);
        }

        public void CreateUser(Guid id, string password)
        {
            var succeeded = _userDirectoryProvider.CreateUser(id.ToString(), password);

            CheckAndThrowFailureError(succeeded, "User creation failed");
        }

        public void ResetUserPassword(Guid id, string password)
        {
            var succeeded = _userDirectoryProvider.ResetPassword(id.ToString(), password);

            CheckAndThrowFailureError(succeeded, "Reset user password failed");
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
