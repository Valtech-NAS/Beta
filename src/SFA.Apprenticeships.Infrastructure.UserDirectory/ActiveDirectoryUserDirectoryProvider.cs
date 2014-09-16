namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using ActiveDirectory;
    using Application.Authentication;
    using Domain.Entities.Exceptions;
    using NLog;

    public class ActiveDirectoryUserDirectoryProvider : IUserDirectoryProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ActiveDirectoryChangePassword _changePassword;
        private readonly ActiveDirectoryServer _server;

        public ActiveDirectoryUserDirectoryProvider(ActiveDirectoryServer server,
            ActiveDirectoryChangePassword changePassword)
        {
            _server = server;
            _changePassword = changePassword;
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
                    Enabled = false,
                };

                // create the account
                userPrincipal.Save();

                // set initial password
                if (!SetUserPassword(userId, null, password))
                {
                    Logger.Debug("Set password for user with Id={0} failed", userId);
                    return false;
                }

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
                            Logger.Debug("Failed setting user password for active directory account userId={0}", userId);
                            return false;
                        }
                        user.Enabled = true;
                        user.Save();

                        Logger.Debug("Successfully reseted password for userId={0}.", userId);
                        return true;
                    }
                    catch (PasswordException exception)
                    {
                        var message = string.Format("SetPassword failed for user {0}", userId);
                        Logger.ErrorException(message, exception);
                        throw;
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
            if (!_server.Bind()) return false;

            Logger.Debug("Set user password for active directory account userId={0}", userId);

            var rs = _changePassword.Change(userId, oldPassword, newPassword);
            return (rs.ResultCode == ResultCode.Success);
        }
    }
}