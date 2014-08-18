namespace TestUserCreator.Process
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using NLog;

    public class CreateTestUsersProcess
    {
        private const string LdapServerPolicyHintsOid = "1.2.840.113556.1.4.2066";

        private const string UserNameFormat = "00000000-0000-0000-0000-000000000{0:D3}";
        private const string DefaultPassword = "?Password01!";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly LdapConnection _connection;
        private readonly PrincipalContext _context;
        private readonly ActiveDirectoryConfig _config;

        public CreateTestUsersProcess(ActiveDirectoryConfig config)
        {
            _config = config;
            _connection = CreateConnection(_config);
            _context = CreateContext(_config);
        }

        public void Run(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                var username = string.Format(UserNameFormat, i);

                if (UserPrincipal.FindByIdentity(_context, username) == null)
                {
                    Logger.Info("User does not exist (\"{0}\"), creating", username);

                    var userPrincipal = CreateUser(username);

                    SetPassword(username, DefaultPassword);
                    EnableUser(userPrincipal);
                }
                else
                {
                    Logger.Info("User exists (\"{0}\"), nothing to do ", username);                    
                }
            }
        }

        private UserPrincipal CreateUser(string username)
        {
            var userPrincipal = new UserPrincipal(_context)
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

            userPrincipal.Save();

            return userPrincipal;
        }

        private static void EnableUser(UserPrincipal userPrincipal)
        {
            userPrincipal.Enabled = true;
            userPrincipal.Save();
        }

        private static LdapConnection CreateConnection(ActiveDirectoryConfig config)
        {
            var identifier = new LdapDirectoryIdentifier(config.Server, config.Port);
            var connection = new LdapConnection(identifier);

            connection.SessionOptions.SecureSocketLayer = true;
            connection.SessionOptions.VerifyServerCertificate = ServerCallback;
            connection.Credential = new NetworkCredential(config.Username, config.Password);
            connection.AuthType = AuthType.Negotiate;

            connection.Bind();

            return connection;
        }

        public static PrincipalContext CreateContext(ActiveDirectoryConfig config)
        {
            var context = new PrincipalContext(
                ContextType.Domain,
                config.Server,
                config.DistinguishedName,
                ContextOptions.Negotiate,
                config.Username,
                config.Password);

            if (!context.ValidateCredentials(config.Username, config.Password, ContextOptions.Negotiate))
            {
                throw new InvalidOperationException("Failed to validate username / password credentials.");
            }

            return context;
        }

        private static bool ServerCallback(LdapConnection connection, X509Certificate certificate)
        {
            return true;
        }

        private void SetPassword(string username, string newPassword)
        {
            var distinguishedName = string.Format(@"CN={0},{1}", username, _config.DistinguishedName);

            Logger.Info("Setting password for user: \"{0}\"", username);
            Logger.Info("Distinguished name: \"{0}\"", distinguishedName);

            // the 'unicodePWD' attribute is used to handle pwd handling requests
            const string attribute = "unicodePwd";

            if (!String.IsNullOrEmpty(newPassword))
            {
                // do we have a pwd to set -> set pwd
                var directoryAttributeModificationReplace = new DirectoryAttributeModification
                {
                    Name = attribute,
                    Operation = DirectoryAttributeOperation.Replace
                };

                directoryAttributeModificationReplace.Add(BuildBytePwd(newPassword));

                var damList = new[] { directoryAttributeModificationReplace };
                var modifyRequest = new ModifyRequest(distinguishedName, damList);

                // Should we utilize pwd history on the pwd reset?
                var value = BerConverter.Encode("{i}", new object[] { 0x1 });
                var pwdHistory = new DirectoryControl(LdapServerPolicyHintsOid, value, false, true);

                modifyRequest.Controls.Add(pwdHistory);

                var response = _connection.SendRequest(modifyRequest);

                // TODO: handle bad response.
            }
        }

        private static byte[] BuildBytePwd(string pwd)
        {
            return (Encoding.Unicode.GetBytes(String.Format("\"{0}\"", pwd)));
        }
    }
}
