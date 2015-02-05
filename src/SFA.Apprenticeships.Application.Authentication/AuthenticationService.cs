namespace SFA.Apprenticeships.Application.Authentication
{
    using System;
    using Domain.Entities.Exceptions;
    using Interfaces.Logging;
    using Interfaces.Users;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class AuthenticationService : IAuthenticationService
    {
        private static ILogService _logger;
        private readonly IUserDirectoryProvider _userDirectoryProvider;

        public AuthenticationService(IUserDirectoryProvider userDirectoryProvider, ILogService logger)
        {
            _userDirectoryProvider = userDirectoryProvider;
            _logger = logger;
        }

        public bool AuthenticateUser(Guid userId, string password)
        {
            _logger.Debug("Calling AuthenticationService to authenticate user with Id={0}", userId);
            return _userDirectoryProvider.AuthenticateUser(userId.ToString(), password);
        }

        public void CreateUser(Guid userId, string password)
        {
            _logger.Debug("Calling AuthenticationService to create a new user with Id={0}", userId);
            var succeeded = _userDirectoryProvider.CreateUser(userId.ToString(), password);

            CheckAndThrowFailureError(succeeded, "User creation failed", ErrorCodes.UserCreationError);
        }

        public void ResetUserPassword(Guid userId, string password)
        {
            _logger.Debug("Calling AuthenticationService to reset password for the user with Id={0}", userId);
            var succeeded = _userDirectoryProvider.ResetPassword(userId.ToString(), password);

            CheckAndThrowFailureError(succeeded, "Reset user password failed", ErrorCodes.UserResetPasswordError);
        }

        public void ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            _logger.Debug("Calling AuthenticationService to change the password for the user with Id={0}", userId);
            var succeeded = _userDirectoryProvider.ChangePassword(userId.ToString(), oldPassword, newPassword);

            CheckAndThrowFailureError(succeeded, "Change password failed", ErrorCodes.UserChangePasswordError);
        }

// ReSharper disable once UnusedParameter.Local
        private static void CheckAndThrowFailureError(bool succeeded, string errorMessage, string errorCode)
        {
            if (!succeeded)
            {
                _logger.Debug(errorMessage);
                throw new CustomException(errorMessage, errorCode);
            }
        }
    }
}
