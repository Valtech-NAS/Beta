using System;
using RestSharp;
using RestSharp.Deserializers;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Service
{
    public class ElasticServiceService : IElasticSearchService
    {
        private readonly string _serviceEndpoint;

        public ElasticServiceService(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            _serviceEndpoint = url;
        }

        public IRestResponse Execute(Method method, string command)
        {
            var request = new RestRequest(command, method);
            return Execute(request);
        }

        public IRestResponse Execute(Method method, string command, string id, string json)
        {
            var request = new RestRequest("{command}/{id}", method) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("command", command);
            request.AddUrlSegment("id", id);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            return Execute(request);
        }

        public IRestResponse Execute(string command, string id, string json)
        {
            return Execute(Method.PUT, command, id, json);
        }

        public IRestResponse Execute(IRestRequest request)
        {
            var client = new RestClient { BaseUrl = _serviceEndpoint };
            client.AddHandler("application/json", new JsonDeserializer());

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException(
                    "Error retrieving response. Check inner details for more info.",
                    response.ErrorException);
            }

            return response;
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            var client = new RestClient { BaseUrl = _serviceEndpoint };
            client.AddHandler("application/json", new JsonDeserializer());

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException(
                    "Error retrieving response. Check inner details for more info.",
                    response.ErrorException);
            }

            return response;
        }
    }
}