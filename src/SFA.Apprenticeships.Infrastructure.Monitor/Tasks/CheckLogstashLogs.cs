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
    using RestSharp.Extensions;

    /// <summary>
    /// Checks that Logstash log entries are being created.
    /// </summary>
    internal class CheckLogstashLogs : IMonitorTask
    {
        private const string ExpectedLogCountSettingName = "Monitor.Logstash.ExpectedLogCount";
        private const string ExpectedLogTimeframeInMinutesSettingName = "Monitor.Logstash.ExpectedLogTimeframeInMinutes";
        private const string TimeoutSettingName = "Monitor.Logstash.Timeout";
        private const string BaseUrlSettingName = "Monitor.Logstash.BaseUrl";

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
            EnsureExpectedNumberOfMessagesLoggedInTimeframe();
        }

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

            // Look for log entries for today and yesterday to handle midnight boundary.
            foreach (var indexDate in new[] { DateTime.Today, DateTime.Today.AddDays(-1) })
            {
                var uri = BuildUri(indexDate);
                var request = CreateRestRequest(uri);

                var response = client.Execute<dynamic>(request);

                EnsureResponseStatusCodeIsOk(response);
                EnsureResponseHasData(response);

                var logEntries = response.Data.hits.hits;

                foreach (var logEntry in logEntries)
                {
                    // It's whacky but the JSON 'path' to a timestamp value looks like this:
                    // response.Data.hits.hits[0].fields["@timestamp"][0].Value
                    timestamps.Add(logEntry.fields["@timestamp"][0].Value);
                }

                if (timestamps.Count >= ExpectedMinimumLogCount)
                {
                    // We have enough log entries to check.
                    break;
                }
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
                var message = string.Format(
                    "Logstash query returned HTTP status code {0}.", response.StatusCode);

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
    }
}