namespace SFA.Apprenticeships.Services.Elasticsearch.Service
{
    using System;
    using System.Collections.Generic;
    using RestSharp;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.Rest;
    using SFA.Apprenticeships.Services.Elasticsearch.Interfaces;

    public class ElasticsearchService : RestService, IElasticsearchService
    {
        public const string ElasticsearchEndpointKey = "ElasticsearchEndpoint";

        public ElasticsearchService(IConfigurationManager config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            BaseUrl = config.TryGetAppSetting(ElasticsearchEndpointKey);
            if (string.IsNullOrEmpty(BaseUrl))
            {
                throw new ArgumentException("AppSetting ElasticsearchEndpoint was not found in the config file.");
            }
        }

        public ElasticsearchService(string url)
            : base(url)
        {
        }

        public IRestResponse Execute(Method method, string command)
        {
            var request = Create(method, command);
            return Execute(request);
        }

        public IRestResponse Execute(Method method, string index, string document, string id, string json)
        {
            var request = Create(
                method,
                "{index}/{document}/{id}",
                json,
                new[]
                {
                    new KeyValuePair<string, string>("index", index),
                    new KeyValuePair<string, string>("document", document),
                    new KeyValuePair<string, string>("id", id),
                });

            return Execute(request);
        }

        public IRestResponse Execute(string index, string document, string id, string json)
        {
            return Execute(Method.PUT, index, document, id, json);
        }
    }
}