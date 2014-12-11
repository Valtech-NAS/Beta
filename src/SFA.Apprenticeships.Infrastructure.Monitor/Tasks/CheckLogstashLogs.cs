using EasyNetQ.Management.Client.Model;
using Elasticsearch.Net;
using Nest;

namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Domain.Interfaces.Configuration;
    using Newtonsoft.Json;
    using NLog;
    using RestSharp;
    using RestSharp.Deserializers;

    /// <summary>
    /// Checks that Logstash log entries are being created.
    /// </summary>
    internal class CheckLogstashLogs : IMonitorTask
    {
        private const string ExpectedLogCountSettingName = "Monitor.Logstash.ExpectedLogCount";
        private const string ExpectedLogTimeframeInMinutesSettingName = "Monitor.Logstash.ExpectedLogTimeframeInMinutes";
        private const string TimeoutSettingName = "Monitor.Logstash.Timeout";
        private const string BaseUrlSettingName = "Monitor.Logstash.BaseUrl";
        private const string NodeCountSettingName = "Monitor.Logstash.NodeCount";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConfigurationManager _configurationManager;

        public class DynamicJsonDeserializer : IDeserializer
        {
            public string RootElement { get; set; }
            public string Namespace { get; set; }
            public string DateFormat { get; set; }

            public T Deserialize<T>(IRestResponse response)
            {
                return JsonConvert.DeserializeObject<dynamic>(response.Content);
            }
        }

        public CheckLogstashLogs(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string TaskName
        {
            get { return "Check Logstash Logs"; }
        }

        public void Run()
        {
            EnsureClusterIsHealthy();
            EnsureExpectedNumberOfMessagesLoggedInTimeframe();            
        }

        #region Querying Logstash Tests

        private void EnsureExpectedNumberOfMessagesLoggedInTimeframe()
        {
            var timeframeStart = DateTime.Now.AddMinutes(-ExpectedTimeframeInMinutes);

            var timestamps = GetRecentLogEntryTimestamps();
            var actualLogCount = timestamps.Count(timestamp => timestamp >= timeframeStart);

            Logger.Debug("Looking for {0} Logstash message(s) in last {1} minute(s), saw {2}.",
                ExpectedMinimumLogCount, ExpectedTimeframeInMinutes, actualLogCount);

            if (actualLogCount >= ExpectedMinimumLogCount)
            {
                return;
            }

            var message = string.Format(
                "Expected {0} Logstash message(s) in last {1} minute(s), saw {2}.",
                ExpectedMinimumLogCount, ExpectedTimeframeInMinutes, actualLogCount);

            throw new Exception(message);
        }

        private IEnumerable<DateTime> GetRecentLogEntryTimestamps()
        {
            var client = CreateRestClient();
            var timestamps = new List<DateTime>();

            var indexDate = DateTime.Now.AddMinutes(-30);
            var uri = BuildUri(indexDate);
            var request = CreateRestRequest(uri);

            var response = client.Execute<dynamic>(request);
            if (response == null)
            {
                throw new Exception("Logstash query returned a null response");
            }
                
            EnsureResponseStatusCodeIsOk(response);
            EnsureResponseHasData(response);

            var logEntries = response.Data.hits.hits;

            foreach (var logEntry in logEntries)
            {
                // It's whacky but the JSON 'path' to a timestamp value looks like this:
                // response.Data.hits.hits[0].fields["@timestamp"][0].Value
                timestamps.Add(logEntry.fields["@timestamp"][0].Value);
            }

            return timestamps;
        }

        private static void EnsureResponseHasData(IRestResponse<dynamic> response)
        {
            if (response.Data == null || response.Data.hits == null || response.Data.hits.hits == null)
            {
                throw new Exception("Logstash query returned no log entry data.");
            }
        }

        private static void EnsureResponseStatusCodeIsOk(IRestResponse<dynamic> response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = string.Format("Logstash query returned HTTP status code {0}.", response.StatusCode);

                throw new Exception(message);
            }
        }

        private static RestClient CreateRestClient()
        {
            var client = new RestClient();

            client.AddHandler("application/json", new DynamicJsonDeserializer());

            return client;
        }

        public virtual IRestRequest CreateRestRequest(string uri)
        {
            var request = new RestRequest(uri, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                Timeout = Timeout * 1000
            };

            request.AddHeader("Accept", "application/json");

            return request;
        }

        private string BuildUri(DateTime indexDate)
        {
            const string format = "{0}/logstash-{1}/logstash/_search?sort=@timestamp:desc&fields=@timestamp&size={2}";
            var uri = string.Format(format, BaseUrl, indexDate.ToString("yyyy.MM.dd"), ExpectedMinimumLogCount);

            return uri;
        }

        #endregion

        #region Query Elasticsearch Management API Tests

        private void EnsureClusterIsHealthy()
        {
            var health = GetClusterHealth();
            EnsureNoTimeout(health);
            EnsureExpectedNumberOfNodes(health);
            EnsureClusterIsHealthy(health);
        }

        private void EnsureNoTimeout(IHealthResponse health)
        {
            if (!health.TimedOut)
            {
                return;
            }

            var message = string.Format("Logstash elastic cluster health check timed out ({0}).", Timeout);

            throw new Exception(message);
        }

        private void EnsureExpectedNumberOfNodes(IHealthResponse response)
        {
            if (NodeCount == response.NumberOfNodes) { return; }
            var message = string.Format("Expected {0} Elasticsearch node(s), saw {1}.", NodeCount, response.NumberOfNodes);
            throw new Exception(message);
        }

        private void EnsureClusterIsHealthy(IHealthResponse health)
        {
            if (health.Status == "green") { return; }

            if (health.Status == "yellow" && health.NumberOfNodes == 1 && NodeCount == health.NumberOfNodes)
            {
                return;
            }

            var message = string.Format("Cluster is unhealthy: \"{0}\", cluster should contain {1} nodes, but only has {2}.", health.Status, NodeCount, health.NumberOfNodes);

            Logger.Warn(message);
        }

        private IHealthResponse GetClusterHealth()
        {
            var request = new ClusterHealthRequest
            {
                Level = Level.Cluster,
                //http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-health.html
                WaitForStatus = WaitForStatus.Yellow,
                Timeout = Timeout.ToString()
            };

            var client = new ElasticClient(new ConnectionSettings(new Uri(BaseUrl)));
            return client.ClusterHealth(request);
        }

        #endregion

        #region Settings

        private int ExpectedMinimumLogCount
        {
            get
            {
                return _configurationManager.GetAppSetting<int>(ExpectedLogCountSettingName);
            }
        }

        private int ExpectedTimeframeInMinutes
        {
            get
            {
                return _configurationManager.GetAppSetting<int>(ExpectedLogTimeframeInMinutesSettingName);
            }
        }

        private int Timeout
        {
            get
            {
                return _configurationManager.GetAppSetting<int>(TimeoutSettingName);
            }
        }

        private string BaseUrl
        {
            get
            {
                return _configurationManager.GetAppSetting(BaseUrlSettingName);
            }
        }

        private int NodeCount
        {
            get
            {
                return _configurationManager.GetAppSetting<int>(NodeCountSettingName);
            }
        }

        #endregion
    }
}