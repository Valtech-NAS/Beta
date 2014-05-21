using RestSharp;
using SFA.Apprenticeships.Common.Interfaces.Services;

namespace SFA.Apprenticeships.Common.Interfaces.Elasticsearch
{
    public interface IElasticsearchService : IRestService
    {
        /// <summary>
        /// The command string to pass in the restful url. eg. \vacancies\vacancy.
        /// The method to use for the api call.
        /// </summary>
        IRestResponse Execute(Method method, string command);

        /// <summary>
        /// Uses a PUT to send the api 'command' with appended 'id' to the endpoint.
        /// Attaches the json object to the request body.
        /// </summary>
        IRestResponse Execute(string index, string document, string id, string json);

        /// <summary>
        /// Uses 'Method' to send the api 'command' with appended 'id' to the endpoint.
        /// Attaches the json object to the request body.
        /// </summary>
        IRestResponse Execute(Method method, string index, string document, string id, string json);
    }
}
