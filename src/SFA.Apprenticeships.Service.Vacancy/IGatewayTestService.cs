namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.ServiceModel;

    //TODO: remove this temporary contract and the service proxy
    [ServiceContract]
    public interface IGatewayTestService
    {
        [OperationContract]
        string ConnectionTest(string endpointAddress, string configurationName, bool listCerts);
    }
}
