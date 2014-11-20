namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System;
    using Application.Authentication;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Hash;
    using NLog;
    using Repositories.Authentication;
    using Repositories.Authentication.Entities;
    using UsersErrorCodes = Application.Interfaces.Users.ErrorCodes;

    public class UserDirectoryProvider : IUserDirectoryProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConfigurationManager _configurationManager;
        private readonly IAuthenticationRepository _repository;
        private readonly IPasswordHash _passwordHash;

        public UserDirectoryProvider(
            IConfigurationManager configurationManager,
            IAuthenticationRepository repository,
            IPasswordHash passwordHash)
        {
            _configurationManager = configurationManager;
            _repository = repository;
            _passwordHash = passwordHash;
        }

        public bool AuthenticateUser(string userId, string password)
        {
            Logger.Debug("Calling user directory to authenticate user with userId={0}", userId);

            Logger.Debug("Checking if user with Id={0} exists on Mongo", userId);
            var userCredentials = _repository.Get(new Guid(userId), true);

            Logger.Debug("User with Id={0} found on Mongo", userId);

            var isValidated = _passwordHash.Validate(userCredentials.PasswordHash, userId, password, SecretKey);

            var message = isValidated
                        ? "successfully validated credentials for Id={0}"
                        : "failed to validate credentials for Id={0}";

            Logger.Debug(message, userId);

            return isValidated;
        }

        public bool CreateUser(string userId, string password)
        {
            Logger.Debug("Calling user directory to create LDAP account for user with Id={0}", userId);

            Logger.Debug("Checking if a user directory account already exist for user with Id={0}", userId);

            var userCredentials = _repository.Get(new Guid(userId));
            if (userCredentials != null)
            {
                var message = string.Format("User directory account for user with Id = \"{0}\" already exists", userId);
                Logger.Debug(message);
                throw new CustomException(message, UsersErrorCodes.UserDirectoryAccountExistsError);
            }

            Logger.Debug("No user directory account found for user with Id={0}", userId);

            var hash = _passwordHash.Generate(userId, password, SecretKey);

            Logger.Debug("LDAP account for user with Id={0} has been successfully created", userId);

            _repository.Save(new UserCredentials
            {
                EntityId = new Guid(userId),
                PasswordHash = hash
            });

            Logger.Debug("User directory account for user with Id={0} has been successfully created", userId);

            return true;
        }

        public bool ResetPassword(string userId, string newpassword)
        {
            Logger.Debug("Calling user directory to reset password for user with Id={0}", userId);

            return SetUserPassword(userId, newpassword);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            Logger.Debug("Change password for user directory account userId={0}", userId);

            return AuthenticateUser(userId, oldPassword) && SetUserPassword(userId, newPassword);
        }

        private bool SetUserPassword(string userId, string newPassword)
        {
            Logger.Debug("Checking if user with Id={0} exists on Mongo", userId);
            var userCredentials = _repository.Get(new Guid(userId), true);
            var hash = _passwordHash.Generate(userId, newPassword, SecretKey);
            userCredentials.PasswordHash = hash;
            Logger.Debug("Saving the new password for user with Id={0} on Mongo", userId);
            _repository.Save(userCredentials);
            return true;
        }

        private string SecretKey
        {
            get { return _configurationManager.GetAppSetting("UserDirectory.SecretKey"); }
        }
    }
}