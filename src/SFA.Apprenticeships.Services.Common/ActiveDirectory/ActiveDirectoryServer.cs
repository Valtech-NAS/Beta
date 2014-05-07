using System;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using SFA.Apprenticeships.Services.Common.Configuration;

namespace SFA.Apprenticeships.Services.Common.ActiveDirectory
{
    public class ActiveDirectoryServer : IDisposable
    {
        private const int ValidationFailed = 49;
        private readonly bool _isSecure;
        private readonly string _username;
        private readonly string _password;

        public ActiveDirectoryServer(IConfigurationManager configManager, bool isSecure)
        {
            _isSecure = isSecure;

            Server = configManager.GetAppSetting<string>("ActiveDirectoryServer");
            DistinguishedName = configManager.GetAppSetting<string>("ActiveDirectoryDistinguishedName");
            _username = configManager.GetAppSetting<string>("ActiveDirectoryUsername");
            _password = configManager.GetAppSetting<string>("ActiveDirectoryPassword");
            Port = isSecure
                ? configManager.GetAppSetting<int>("ActiveDirectorySecurePort")
                : configManager.GetAppSetting<int>("ActiveDirectoryPort");

            Connection = new LdapConnection(new LdapDirectoryIdentifier(Server, Port));
            Connection.SessionOptions.SecureSocketLayer = isSecure;
            Connection.SessionOptions.VerifyServerCertificate = ServerCallback;
        }

        public string Server { get; private set; }
        public string DistinguishedName { get; private set; }
        public int Port { get; private set; }
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
                        _username,
                        _password);
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
