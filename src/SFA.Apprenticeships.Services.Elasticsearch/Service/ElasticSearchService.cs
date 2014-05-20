using System;
using System.Collections.Generic;
using RestSharp;
using SFA.Apprenticeships.Common.Configuration;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.Common.Rest;

namespace SFA.Apprenticeships.Services.Elasticsearch.Service
{
    public class ElasticsearchService : RestService, IElasticSearchService
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

        public IRestResponse Execute(Method method, string command, string id, string json)
        {
            var request = Create(
                method, 
                "{command}/{id}", 
                json,
                new[]
                {
                    new KeyValuePair<string, string>("command", command), 
                    new KeyValuePair<string, string>("id", id),
                });

            return Execute(request);
        }

        public IRestResponse Execute(string command, string id, string json)
        {
            return Execute(Method.PUT, command, id, json);
        }
    }
}