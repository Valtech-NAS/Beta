namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using GatewayServiceProxy;

    //TODO: remove this temporary contract and the service proxy
    public class GatewayTestService : IGatewayTestService
    {
        public string ConnectionTest(string endpointAddress, string configurationName, bool listCerts)
        {
            var log = new StringBuilder();

            try
            {
                if (listCerts)
                {
                    ListCertificates(StoreLocation.LocalMachine, StoreName.My, log);
                    ListCertificates(StoreLocation.LocalMachine, StoreName.Root, log);
                    ListCertificates(StoreLocation.LocalMachine, StoreName.AuthRoot, log);
                    ListCertificates(StoreLocation.LocalMachine, StoreName.CertificateAuthority, log);
                    ListCertificates(StoreLocation.CurrentUser, StoreName.My, log);
                    ListCertificates(StoreLocation.CurrentUser, StoreName.Root, log);
                    ListCertificates(StoreLocation.CurrentUser, StoreName.AuthRoot, log);
                    ListCertificates(StoreLocation.CurrentUser, StoreName.CertificateAuthority, log);
                }

                log.AppendLine("Calling '" + endpointAddress + "' with configuration '" + configurationName + "'");

                GetServiceStatusResponse response = null;
                Service<GatewayServiceContract>.Use(configurationName, endpointAddress, client => { response = client.GetServiceStatus(new GetServiceStatusRequest()); });

                log.AppendLine("Response: " + response.Version);
            }

            catch (Exception ex)
            {
                log.AppendFormat("Error: {0}\n{1}\n", ex.Message, ex.StackTrace);
            }

            return log.ToString();
        }

        private static void ListCertificates(StoreLocation storeLocation, StoreName storeName, StringBuilder log)
        {
            log.AppendFormat("Certificates in store {0} at {1}\n", storeName, storeLocation);

            var store = new X509Store(storeName);

            store.Open(OpenFlags.ReadOnly);

            foreach (var cert in store.Certificates)
            {
                log.AppendFormat("...cert: fn='{0}', sn='{1}', in='{2}', th='{3}'\n", cert.FriendlyName, cert.SubjectName, cert.IssuerName, cert.Thumbprint);
            }
        }
    }
}
