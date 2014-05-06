using RestSharp;
using SFA.Apprenticeships.Repository.Elasticsearch.Entities;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Abstract
{
    public interface IElasticSearchService
    {
        /// <summary>
        /// The command string to pass in the restful url. eg. \vacancies\vacancy.
        /// The method to use for the api call.
        /// </summary>
        IRestResponse SendCommand(Method method, string command);

        /// <summary>
        /// Uses a PUT to send the api 'command' with appended 'id' to the endpoint.
        /// Attaches the json object to the request body.
        /// </summary>
        IRestResponse SendCommand(string command, string id, string json);

        /// <summary>
        /// Uses 'Method' to send the api 'command' with appended 'id' to the endpoint.
        /// Attaches the json object to the request body.
        /// </summary>
        IRestResponse SendCommand(Method method, string command, string id, string json);

        IRestResponse Send(IRestRequest request);

        IRestResponse<T> Send<T>(IRestRequest request) where T : new();
    }
}
