using RestSharp;

namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
{
    public interface IElasticSearchService
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
        IRestResponse Execute(string command, string id, string json);

        /// <summary>
        /// Uses 'Method' to send the api 'command' with appended 'id' to the endpoint.
        /// Attaches the json object to the request body.
        /// </summary>
        IRestResponse Execute(Method method, string command, string id, string json);

        /// <summary>
        /// Executes the request.
        /// </summary>
        IRestResponse Execute(IRestRequest request);

        /// <summary>
        /// Executes the request and returns response data of type T
        /// </summary>
        IRestResponse<T> Execute<T>(IRestRequest request) where T : new();
    }
}
