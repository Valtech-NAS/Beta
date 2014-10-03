namespace SFA.Apprenticeships.Application.Authentication
{
    using System;
    using Domain.Entities.Exceptions;
    using Interfaces.Users;
    using NLog;

    public class AuthenticationService : IAuthenticationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUserDirectoryProvider _userDirectoryProvider;

        public AuthenticationService(IUserDirectoryProvider userDirectoryProvider)
        {
            _userDirectoryProvider = userDirectoryProvider;
        }

        public bool AuthenticateUser(Guid userId, string password)
        {
            Logger.Debug("Calling AuthenticationService to authenticate user with Id={0}", userId);
            return _userDirectoryProvider.AuthenticateUser(userId.ToString(), password);
        }

        public void CreateUser(Guid userId, string password)
        {
            Logger.Debug("Calling AuthenticationService to create a new user with Id={0}", userId);
            var succeeded = _userDirectoryProvider.CreateUser(userId.ToString(), password);

            CheckAndThrowFailureError(succeeded, "User creation failed", Domain.Entities.Exceptions.ErrorCodes.UserCreationError);
        }

        public void ResetUserPassword(Guid userId, string password)
        {
            Logger.Debug("Calling AuthenticationService to reset password for the user with Id={0}", userId);
            var succeeded = _userDirectoryProvider.ResetPassword(userId.ToString(), password);

            CheckAndThrowFailureError(succeeded, "Reset user password failed", Domain.Entities.Exceptions.ErrorCodes.UserResetPasswordError);
        }

        public void ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            Logger.Debug("Calling AuthenticationService to change the password for the user with Id={0}", userId);
            var succeeded = _userDirectoryProvider.ChangePassword(userId.ToString(), oldPassword, newPassword);

            CheckAndThrowFailureError(succeeded, "Change password failed", Domain.Entities.Exceptions.ErrorCodes.UserChangePasswordError);
        }

// ReSharper disable once UnusedParameter.Local
        private static void CheckAndThrowFailureError(bool succeeded, string errorMessage, string errorCode)
        {
            if (!succeeded)
            {
                Logger.Debug(errorMessage);
                throw new CustomException(errorMessage, errorCode);
            }
        }
    }
}
