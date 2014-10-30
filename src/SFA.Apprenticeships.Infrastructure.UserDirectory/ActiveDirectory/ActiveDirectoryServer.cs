namespace SFA.Apprenticeships.Infrastructure.UserDirectory.ActiveDirectory
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using System.Net;
    using Configuration;
    using NLog;

    public class ActiveDirectoryServer : IDisposable
    {
        private const int ValidationFailed = 49;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ActiveDirectoryConfiguration _config;
        private readonly bool _isSecure;

        public ActiveDirectoryServer(ActiveDirectoryConfiguration config)
        {
            _config = config;
            _isSecure = _config.SecureMode;
            Connection = new LdapConnection(new LdapDirectoryIdentifier(Server, Port));
            Connection.SessionOptions.SecureSocketLayer = _isSecure;
            Connection.SessionOptions.VerifyServerCertificate = (s, e) => true;
        }

        public string Server
        {
            get { return _config.Server; }
        }

        public string DistinguishedName
        {
            get { return _config.DistinguishedName; }
        }

        public int Port
        {
            get { return _isSecure ? _config.SslPort : _config.Port; }
        }

        public LdapConnection Connection { get; private set; }

        public PrincipalContext Context
        {
            get
            {
                if (_isSecure)
                {
                    return new PrincipalContext(
                        ContextType.Domain,
                        Server,
                        DistinguishedName,
                        ContextOptions.Negotiate,
                        _config.Username,
                        _config.Password);
                }

                return new PrincipalContext(ContextType.Domain, Server);
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public bool Bind()
        {
            Logger.Debug("Binding to LDAP using credentials from configuration.");

            Connection.Credential = new NetworkCredential(_config.Username, _config.Password);
            Connection.AuthType = AuthType.Negotiate;

            try
            {
                Connection.Bind();
                Logger.Debug("Succeeded in binding to LDAP.");
                return true;
            }
            catch (LdapException ldapException)
            {
                Logger.Debug("Failed to bind to LDAP: ", ldapException);

                if (ldapException.ErrorCode != ValidationFailed) { throw; }

                Logger.Debug("Failed to bind to LDAP because validation failed with ErrorCode={0}",
                    ldapException.ErrorCode);
                return false;
            }
        }
    }
}