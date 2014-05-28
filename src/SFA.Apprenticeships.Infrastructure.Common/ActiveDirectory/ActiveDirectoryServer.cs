namespace SFA.Apprenticeships.Infrastructure.Common.ActiveDirectory
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.Protocols;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;

    public class ActiveDirectoryServer : IDisposable
    {
        private const int ValidationFailed = 49;
        private readonly bool _isSecure;
        private readonly IActiveDirectoryConfiguration _config;

        public ActiveDirectoryServer(IActiveDirectoryConfiguration config, bool isSecure)
        {
            if (_config == null)
            {
                throw new ArgumentNullException("config");
            }

            _isSecure = isSecure;
            _config = config;

            Connection = new LdapConnection(new LdapDirectoryIdentifier(Server, Port));
            Connection.SessionOptions.SecureSocketLayer = isSecure;
            Connection.SessionOptions.VerifyServerCertificate = ServerCallback;
        }

        public string Server { get { return _config.Server; } }

        public string DistinguishedName { get { return _config.DistinguishedName; } }

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

        public bool Bind(string username, string password)
        {
            Connection.Credential = new NetworkCredential(username, password);
            Connection.AuthType = AuthType.Negotiate;

            try
            {
                Connection.Bind();
                return true;
            }
            catch (LdapException ldapException)
            {
                if (ldapException.ErrorCode == ValidationFailed)
                {
                    return false;
                }

                throw;
            }
        }

        private static bool ServerCallback(LdapConnection connection, X509Certificate certificate)
        {
            // TODO::High::Validate certificate
            return true;
            //try
            //{
            //    // Need to create a Config manager library and use an interface for this
            //    var localCertificate = ConfigurationManager.AppSettings["AdCertificaate"];
            //    X509Certificate expectedCert = X509Certificate.CreateFromCertFile(localCertificate);

            //    return expectedCert.Equals(certificate);

            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
