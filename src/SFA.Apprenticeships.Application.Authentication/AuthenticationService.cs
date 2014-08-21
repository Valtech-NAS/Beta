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

        public bool AuthenticateUser(Guid userId, string password)
        {
            return _userDirectoryProvider.AuthenticateUser(userId.ToString(), password);
        }

        public void CreateUser(Guid userId, string password)
        {
            var succeeded = _userDirectoryProvider.CreateUser(userId.ToString(), password);

            CheckAndThrowFailureError(succeeded, "User creation failed");
        }

        public void ResetUserPassword(Guid userId, string password)
        {
            var succeeded = _userDirectoryProvider.ResetPassword(userId.ToString(), password);

            CheckAndThrowFailureError(succeeded, "Reset user password failed");
        }

        public void ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var succeeded = _userDirectoryProvider.ChangePassword(userId.ToString(), oldPassword, newPassword);

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
