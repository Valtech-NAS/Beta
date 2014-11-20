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
            var userCredentials = _repository.Get(new Guid(userId));
            if (userCredentials == null)
            {
                var message = string.Format("User directory account for user with Id = \"{0}\" doesn't exist", userId);
                Logger.Debug(message);
                throw new CustomException(message, UsersErrorCodes.UserDirectoryAccountDoesNotExistError);
            }

            return _passwordHash.Validate(userCredentials.PasswordHash, userId, password, SecretKey);
        }

        public bool CreateUser(string userId, string password)
        {
            var userCredentials = _repository.Get(new Guid(userId));
            if (userCredentials != null)
            {
                var message = string.Format("User directory account for user with Id = \"{0}\" already exists", userId);
                Logger.Debug(message);
                throw new CustomException(message, UsersErrorCodes.UserDirectoryAccountExistsError);
            }

            var hash = _passwordHash.Generate(userId, password, SecretKey);

            _repository.Save(new UserCredentials
            {
                EntityId = new Guid(userId),
                PasswordHash = hash
            });

            return true;
        }

        public bool ResetPassword(string userId, string newpassword)
        {
            return SetUserPassword(userId, newpassword);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            return AuthenticateUser(userId, oldPassword) && SetUserPassword(userId, newPassword);
        }

        private bool SetUserPassword(string userId, string newPassword)
        {
            var userCredentials = _repository.Get(new Guid(userId));
            var hash = _passwordHash.Generate(userId, newPassword, SecretKey);
            userCredentials.PasswordHash = hash;
            _repository.Save(userCredentials);
            return true;
        }

        private string SecretKey
        {
            get { return _configurationManager.GetAppSetting("UserDirectory.SecretKey"); }
        }
    }
}