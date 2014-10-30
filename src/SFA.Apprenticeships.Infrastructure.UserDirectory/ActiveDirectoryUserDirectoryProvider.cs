namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using ActiveDirectory;
    using Application.Authentication;
    using Configuration;
    using Domain.Entities.Exceptions;
    using NLog;

    public class ActiveDirectoryUserDirectoryProvider : IUserDirectoryProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ActiveDirectoryServer _server;
        private readonly ActiveDirectoryConfiguration _config;

        public ActiveDirectoryUserDirectoryProvider(
            ActiveDirectoryServer server,
            ActiveDirectoryConfiguration config)
        {
            _server = server;
            _config = config;
        }

        public bool AuthenticateUser(string userId, string password)
        {
            Logger.Debug("Calling active directory to authenticate user with userId={0}", userId);

            using (var context = _server.Context)
            {
                Logger.Debug("Checking if user with Id={0} exists on LDAP", userId);

                var user = UserPrincipal.FindByIdentity(context, userId);

                if (user != null)
                {
                    Logger.Debug("User with Id={0} found on LDAP", userId);

                    var isValidated = context.ValidateCredentials(user.UserPrincipalName, password);

                    var message = isValidated
                        ? "LDAP successfully validated credentials for Id={0}"
                        : "LDAP failed to validate credentials for Id={0}";

                    Logger.Debug(message, userId);

                    return isValidated;
                }

                Logger.Debug("User with Id={0} was not found on LDAP", userId);

                return false;
            }
        }

        public bool CreateUser(string userId, string password)
        {
            Logger.Debug("Calling active directory to create LDAP account for user with Id={0}", userId);

            using (var context = _server.Context)
            {
                Logger.Debug("Checking if an LDAP account already exist for user with Id={0}", userId);

                var user = UserPrincipal.FindByIdentity(context, userId);

                if (user != null)
                {
                    var message = string.Format("LDAP account for user with Id={0} already exist", userId);
                    Logger.Debug(message);
                    throw new CustomException(message, ErrorCodes.LdapAccountExistError);
                }

                Logger.Debug("No LDAP account found for user with Id={0}", userId);

                // Create the new UserPrincipal object
                var userPrincipal = new UserPrincipal(context)
                {
                    PasswordNeverExpires = true,
                    PasswordNotRequired = false,
                    UserCannotChangePassword = false,
                    Surname = userId,
                    GivenName = userId,
                    Name = userId,
                    UserPrincipalName = userId,
                    Enabled = false
                };

                // Create the account.
                userPrincipal.Save();

                // Set the password.
                if (!SetUserPassword(userId, null, password))
                {
                    Logger.Debug("Set password for user with Id={0} failed", userId);
                    return false;
                }

                // Enable the account.
                userPrincipal.Enabled = true;
                userPrincipal.Save();

                Logger.Debug("LDAP account for user with Id={0} has been successfully created", userId);

                return true;
            }
        }

        public bool ResetPassword(string userId, string newpassword)
        {
            Logger.Debug("Calling LDAP to reset password for user with Id={0}", userId);

            using (var context = _server.Context)
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.UserPrincipalName, userId))
                {
                    if (user == null)
                    {
                        var message = string.Format("No active directory account found for userId={0}", userId);
                        Logger.Debug(message);
                        throw new CustomException(message, ErrorCodes.UnknownUserError);
                    }

                    try
                    {
                        if (!SetUserPassword(userId, null, newpassword))
                        {
                            Logger.Debug("Failed to set password for LDAP account with userId={0}", userId);
                            return false;
                        }
                        user.Enabled = true;
                        user.Save();

                        Logger.Debug("Successfully reset password for LDAP account with userId={0}.", userId);
                        return true;
                    }
                    catch (PasswordException exception)
                    {
                        var message = string.Format("Set password failed for user {0}", userId);
                        Logger.Error(message, exception);
                        throw new CustomException(message, exception, ErrorCodes.LdapSetPasswordError);
                    }
                }
            }
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            Logger.Debug("Change password for active directory account userId={0}", userId);

            return AuthenticateUser(userId, oldPassword) && SetUserPassword(userId, null, newPassword);
        }

        private bool SetUserPassword(string userId, string oldPassword, string newPassword)
        {
            Logger.Debug("Calling LDAP to set password for user with Id={0}", userId);
            
            if (!_server.Bind())
            {
                Logger.Debug("LDAP server is not bound, cannot set password for userId={0}", userId);
                return false;
            }

            Logger.Debug("Set user password for active directory account userId={0}", userId);

            var changePassword = new ActiveDirectoryChangePassword(_server, _config);

            var rs = changePassword.Change(userId, oldPassword, newPassword);

            Logger.Debug("Change password outcome for user with Id={0} was ResultCode={1}", userId, rs.ResultCode);

            return (rs.ResultCode == ResultCode.Success);
        }
    }
}