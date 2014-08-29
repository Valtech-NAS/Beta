namespace SFA.Apprenticeships.Infrastructure.Postcode.Rest
{
    using System;
    using System.Collections.Generic;
    using RestSharp;

    public interface IRestService
    {
        IRestClient Client { get; set; }
        IRestRequest Create(Method method, string url, string jsonBody = null, params KeyValuePair<string, string>[] segments);
        IRestRequest Create(string url, params KeyValuePair<string, string>[] segments);
        IRestResponse<T> Execute<T>(IRestRequest request) where T : new();
    }
}
