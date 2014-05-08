using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;
using SFA.Apprenticeships.Services.Common.Abstract;

namespace SFA.Apprenticeships.Services.Common.Services
{
    public abstract class RestService : IRestService
    {
        private readonly string _baseUrl;
        private IRestClient _client;

        protected RestService() { }

        protected RestService(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("baseUrl");
            }

            _baseUrl = baseUrl;
        }

        public IRestClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new RestClient { BaseUrl = _baseUrl };
                    (_client as RestClient).AddHandler("application/json", new JsonDeserializer());
                }

                return _client;
            }

            set { _client = value; }
        }

        public virtual IRestRequest Create(
            Method method, 
            string url, 
            string jsonBody = null,
            params KeyValuePair<string, string>[] segments)
        {
            IRestRequest request = new RestRequest(url, method) {RequestFormat = DataFormat.Json};
            if (segments != null)
            {
                foreach (var segment in segments)
                {
                    request.AddUrlSegment(segment.Key, segment.Value);
                }
            }

            if (!string.IsNullOrEmpty(jsonBody))
            {
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            }

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            return request;
        }

        public virtual IRestRequest Create(string url, params KeyValuePair<string, string>[] segments)
        {
            return Create(Method.GET, url, string.Empty, segments);
        }

        public virtual IRestResponse Execute(IRestRequest request)
        {
            var response = _client.Execute(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException(
                    "Error retrieving response. Check inner details for more info.",
                    response.ErrorException);
            }

            return response;
        }

        public virtual IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            var response = _client.Execute<T>(request);

            /* Restsharp derserializer doesnt appear to handle property names that differ from response */
            // var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);

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
