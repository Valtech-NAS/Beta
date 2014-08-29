namespace SFA.Apprenticeships.Infrastructure.Postcode.Rest
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using RestSharp;
    using RestSharp.Deserializers;

    public abstract class RestService : IRestService
    {
        private IRestClient _client;

        protected RestService() { }

        protected RestService(string baseUrl)
        {
            Condition.Requires(baseUrl, "baseUrl").IsNotNullOrWhiteSpace();

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

        public virtual IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            var response = Client.Execute<T>(request);

            /* Restsharp deserializer doesn't appear to handle property names that differ from response */
            // TODO: DEADCODE: var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                ThrowIncompleteResponseException(response);
            }

            if (response.ErrorException != null)
            {
                ThrowErrorException(response);
            }

            return response;
        }

        private static void ThrowIncompleteResponseException<T>(IRestResponse<T> response)
        {
            string message = string.Format(
                "REST service failed to complete: \"{0}\", response status: {1}, HTTP status code: {2}.",
                response.ErrorMessage, response.ResponseStatus, response.StatusCode);

            throw new ApplicationException(message);
        }

        #region Helpers

        private static void ThrowErrorException(IRestResponse response)
        {
            throw new ApplicationException("Error retrieving response. Check inner details for more info.", 
                response.ErrorException);
        }

        #endregion
    }
}
