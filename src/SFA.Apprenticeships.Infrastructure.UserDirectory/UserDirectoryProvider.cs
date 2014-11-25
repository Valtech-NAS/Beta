namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System;
    using Application.Authentication;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Hash;
    using NLog;
    using UsersErrorCodes = Application.Interfaces.Users.ErrorCodes;

    public class UserDirectoryProvider : IUserDirectoryProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConfigurationManager _configurationManager;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordHash _passwordHash;

        public UserDirectoryProvider(
            IConfigurationManager configurationManager,
            IAuthenticationRepository authenticationRepository,
            IPasswordHash passwordHash)
        {
            _configurationManager = configurationManager;
            _authenticationRepository = authenticationRepository;
            _passwordHash = passwordHash;
        }

        public bool AuthenticateUser(string userId, string password)
        {
            Logger.Debug("Authenticating userId={0}", userId);

            var userCredentials = _authenticationRepository.Get(new Guid(userId), true);

            var isValidated = _passwordHash.Validate(userCredentials.PasswordHash, userId, password, SecretKey);

            var message = isValidated
                        ? "Successfully validated credentials for Id={0}"
                        : "Failed to validate credentials for Id={0}";

            Logger.Debug(message, userId);

            return isValidated;
        }

        public bool CreateUser(string userId, string password)
        {
            Logger.Debug("Creating user authentication for userId={0}", userId);

            var userCredentials = _authenticationRepository.Get(new Guid(userId));
            if (userCredentials != null)
            {
                var message = string.Format("User authentication exists for userId={0}", userId);
                Logger.Debug(message);
                throw new CustomException(message, UsersErrorCodes.UserDirectoryAccountExistsError);
            }

            Logger.Debug("No user authentication found for userId={0}", userId);

            var hash = _passwordHash.Generate(userId, password, SecretKey);

            _authenticationRepository.Save(new UserCredentials
            {
                EntityId = new Guid(userId),
                PasswordHash = hash
            });

            Logger.Debug("Created user authentication for userId={0}", userId);

            return true;
        }

        public bool ResetPassword(string userId, string newpassword)
        {
            Logger.Debug("Resetting password for userId={0}", userId);

            return SetUserPassword(userId, newpassword);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            Logger.Debug("Changing password for userId={0}", userId);

            return AuthenticateUser(userId, oldPassword) && SetUserPassword(userId, newPassword);
        }

        private bool SetUserPassword(string userId, string newPassword)
        {
            var userCredentials = _authenticationRepository.Get(new Guid(userId), true);
 
            var hash = _passwordHash.Generate(userId, newPassword, SecretKey);
            userCredentials.PasswordHash = hash;

            Logger.Debug("Saving new password for userId={0}", userId);
            _authenticationRepository.Save(userCredentials);

            return true;
        }

        private string SecretKey
        {
            get { return _configurationManager.GetAppSetting("UserDirectory.SecretKey"); }
        }
    }
}