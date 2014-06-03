namespace SFA.Apprenticeships.Infrastructure.Common.Rest
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using RestSharp;
    using RestSharp.Deserializers;

    public abstract class RestService : IRestService
    {
        private IRestClient _client;

        protected RestService() { }

        protected RestService(string baseUrl)
        {
            Condition.Requires(baseUrl, "baseUrl").IsNotNullOrEmpty();

            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; protected set; }

        public IRestClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new RestClient { BaseUrl = BaseUrl };
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
            var response = Client.Execute(request);

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
            var response = Client.Execute<T>(request);

            /* Restsharp deserializer doesn't appear to handle property names that differ from response */
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
