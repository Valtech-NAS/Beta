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
            return _server.Context.ValidateCredentials(username, password);
        }

        public bool CreateUser(string username, string password)
        {
            UserPrincipal userPrincipal;

            using (var context = _server.Context)
            {
                var user = UserPrincipal.FindByIdentity(context, username);

                if (user != null)
                {
                    throw new Exception("User already exist"); //todo: should use an application exception type
                }
            
                // Create the new UserPrincipal object
                userPrincipal = new UserPrincipal(context)
                {
                    PasswordNeverExpires = true, 
                    PasswordNotRequired = false, 
                    UserCannotChangePassword = false, 
                    Surname = username, 
                    GivenName = username, 
                    SamAccountName = username, 
                    Enabled = false,
                };
            }

            // create the account
            userPrincipal.Save();

            // Use admin cred's
            if (!_server.Bind()) return false;

            var rs = _changePassword.Change(username, null, password);

            if (rs.ResultCode != ResultCode.Success) return false;

            userPrincipal.Enabled = true;

            userPrincipal.Save();

            return true;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!_server.Bind()) return false;

            var rs = _changePassword.Change(username, oldPassword, newPassword);

            return (rs.ResultCode == ResultCode.Success);
        }
    }
}
