namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using Application.Authentication;
    using ActiveDirectory;

    public class ActiveDirectoryUserDirectoryProvider : IUserDirectoryProvider
    {
        private readonly ActiveDirectoryServer _server;
        private readonly ActiveDirectoryChangePassword _changePassword;
        public ActiveDirectoryUserDirectoryProvider(ActiveDirectoryServer server, ActiveDirectoryChangePassword changePassword)
        {
            _server = server;
            _changePassword = changePassword;
        }

        public bool AuthenticateUser(string username, string password)
        {
            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                return user != null && context.ValidateCredentials(user.UserPrincipalName, password);
            }
        }

        public bool CreateUser(string username, string password)
        {
            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                if (user != null)
                {
                    throw new Exception("User already exist"); //todo: should use an application exception type
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

                return true;
            }
        }

        private bool SetUserPassword(string username, string oldPassword, string newPassword)
        {
            if (!_server.Bind()) return false;
            var rs = _changePassword.Change(username, oldPassword, newPassword);
            return (rs.ResultCode == ResultCode.Success);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return AuthenticateUser(username, oldPassword) && SetUserPassword(username, null, newPassword);
        }
    }
}
