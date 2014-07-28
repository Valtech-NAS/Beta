namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using Application.Authentication;
    using ActiveDirectory;
    using NLog;

    public class ActiveDirectoryUserDirectoryProvider : IUserDirectoryProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ActiveDirectoryServer _server;
        private readonly ActiveDirectoryChangePassword _changePassword;
        public ActiveDirectoryUserDirectoryProvider(ActiveDirectoryServer server, ActiveDirectoryChangePassword changePassword)
        {
            _server = server;
            _changePassword = changePassword;
        }

        public bool AuthenticateUser(string username, string password)
        {
            Logger.Debug("Authenticating active directory account for username={0}", username);

            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                return user != null && context.ValidateCredentials(user.UserPrincipalName, password);
            }
        }

        public bool CreateUser(string username, string password)
        {
            Logger.Debug("Creating active directory account for username={0}", username);

            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                if (user != null)
                {
                    Logger.Error("Active directory account for username={0} already exist", username);
                    throw new Exception("User already exist"); // TODO: EXCEPTION: should use an application exception type
                }

                // Create the new UserPrincipal object
                var userPrincipal = new UserPrincipal(context)
                {
                    PasswordNeverExpires = true,
                    PasswordNotRequired = false,
                    UserCannotChangePassword = false,
                    Surname = username,
                    GivenName = username,
                    Name = username,
                    UserPrincipalName = username,
                    Enabled = false,
                };

                // create the account
                userPrincipal.Save();

                // set initial password
                if (!SetUserPassword(username, null, password)) return false;

                userPrincipal.Enabled = true;

                userPrincipal.Save();

                Logger.Debug("Active directory account for username={0} has been successfully created", username);

                return true;
            }
        }

        public bool ResetPassword(string username, string newpassword)
        {
            Logger.Debug("Resetting password for username={0}", username);

            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                if (user == null)
                {
                    Logger.Error("No active directory account found for username={0}", username);
                    throw new Exception("User does not exist"); // TODO: EXCEPTION: should use an application exception type
                }

                try
                {
                    user.SetPassword(newpassword);
                    return true;
                }
                catch (PasswordException exception)
                {
                    var message = string.Format("SetPassword failed for {0}", username);
                    Logger.ErrorException(message, exception);
                    throw;
                }
              
            }
        }

        private bool SetUserPassword(string username, string oldPassword, string newPassword)
        {
            if (!_server.Bind()) return false;

            Logger.Debug("Set user password for active directory account username={0}", username);

            var rs = _changePassword.Change(username, oldPassword, newPassword);
            return (rs.ResultCode == ResultCode.Success);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Logger.Debug("Change password for active directory account username={0}", username);

            return AuthenticateUser(username, oldPassword) && SetUserPassword(username, null, newPassword);
        }
    }
}
