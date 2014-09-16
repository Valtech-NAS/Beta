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
            Logger.Debug("Authenticating active directory account for userId={0}", userId);

            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, userId);

                return user != null && context.ValidateCredentials(user.UserPrincipalName, password);
            }
        }

        public bool CreateUser(string userId, string password)
        {
            Logger.Debug("Creating active directory account for userId={0}", userId);

            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, userId);

                if (user != null)
                {
                    var message = string.Format("Active directory account for userId={0} already exist", userId);
                    Logger.Debug(message);
                    throw new Exception(message);
                    // TODO: EXCEPTION: should use an application exception type
                }

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
                    Logger.Debug("Failed setting user password for active directory account userId={0}", userId);
                    return false;
                }

                userPrincipal.Enabled = true;

                userPrincipal.Save();

                Logger.Debug("Active directory account for userId={0} has been successfully created", userId);

                return true;
            }
        }

        public bool ResetPassword(string userId, string newpassword)
        {
            Logger.Debug("Resetting password for userId={0}", userId);

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