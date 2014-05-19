using System.Collections.Generic;
using RestSharp;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.Common.Rest;

namespace SFA.Apprenticeships.Services.Elasticsearch.Service
{
    public class ElasticServiceService : RestService, IElasticSearchService
    {
        public ElasticServiceService(string url)
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