namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.Configuration;
    using Elasticsearch.Net;
    using Nest;
    using NLog;

    internal class CheckElasticsearchIndexes : IMonitorTask
    {
        private const string ExpectedIndexNamesSettingName = "Monitor.Elasticsearch.ExpectedIndexNames";
        private const string TimeoutSettingName = "Monitor.Elasticsearch.Timeout";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IConfigurationManager _configurationManager;

        public CheckElasticsearchIndexes(IConfigurationManager configurationManager, IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _configurationManager = configurationManager;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public string TaskName
        {
            get { return "Check Elasticsearch Indexes"; }
        }

        public void Run()
        {
            var health = GetClusterHealth();
            var expectedIndexNames = GetExpectedIndexNames();

            EnsureNoTimeout(health);
            EnsureExpectedIndexesExist(health, expectedIndexNames);
            EnsureExpectedIndexesAreHealthy(health, expectedIndexNames);
        }

        private IHealthResponse GetClusterHealth()
        {
            var request = new ClusterHealthRequest
            {
                Level = Level.Indices,
                Timeout = Timeout
            };

            var client = _elasticsearchClientFactory.GetElasticClient();

            return client.ClusterHealth(request);
        }

        private void EnsureNoTimeout(IHealthResponse health)
        {
            if (!health.TimedOut)
            {
                return;
            }

            var message = string.Format(
                "Elasticsearch index check timed out ({0}).", Timeout);

            throw new Exception(message);
        }

        private static void EnsureExpectedIndexesExist(IHealthResponse health, IList<string> expectedIndexNames)
        {
            if (expectedIndexNames.All(each => health.Indices.ContainsKey(each)))
            {
                return;
            }

            var message = string.Format(
                "Found one or more missing Elasticsearch indexes: expected \"{0}\", found \"{1}\".",
                ToCommaSeparatedString(expectedIndexNames), ToCommaSeparatedString(health.Indices.Keys));

            throw new Exception(message);
        }

        private static void EnsureExpectedIndexesAreHealthy(IHealthResponse health, IList<string> expectedIndexNames)
        {
            var unhealthyIndexNames = health.Indices
                .Where(each => expectedIndexNames.Contains(each.Key))
                .Where(each => !IsIndexHealthy(each))
                .Select(each => string.Format("{0} ({1})", each.Key, each.Value.Status))
                .ToList();
            
            if (unhealthyIndexNames.Count == 0)
            {
                return;
            }

            var message = string.Format(
                "Found one or more unhealthy Elasticsearch indexes: \"{0}\".",
                ToCommaSeparatedString(unhealthyIndexNames));

            Logger.Warn(message);
        }

        private static bool IsIndexHealthy(KeyValuePair<string, IndexHealthStats> index)
        {
            return index.Value.Status.Equals("green", StringComparison.InvariantCultureIgnoreCase);
        }

        private IList<string> GetExpectedIndexNames()
        {
            return _configurationManager
                .GetAppSetting(ExpectedIndexNamesSettingName)
                .Split(new[] {';', ','})
                .Select(each => each.Trim().ToLower())
                .ToList();
        }

        private string Timeout
        {
            get
            {
                return string.Format("{0}s",
                    _configurationManager.GetAppSetting<int>(TimeoutSettingName));
            }
        }

        private static string ToCommaSeparatedString(IEnumerable<string> names)
        {
            return string.Join(", ", names);
        }
    }
}
