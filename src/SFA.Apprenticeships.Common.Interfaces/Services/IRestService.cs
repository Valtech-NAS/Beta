using System.Collections.Generic;
using RestSharp;

namespace SFA.Apprenticeships.Common.Interfaces.Services
{
    public interface IRestService
    {
        IRestClient Client { get; set; }
        IRestRequest Create(Method method, string url, string jsonBody = null, params KeyValuePair<string, string>[] segments);
        IRestRequest Create(string url, params KeyValuePair<string, string>[] segments);
        IRestResponse Execute(IRestRequest request);
        IRestResponse<T> Execute<T>(IRestRequest request) where T : new();
    }
}
