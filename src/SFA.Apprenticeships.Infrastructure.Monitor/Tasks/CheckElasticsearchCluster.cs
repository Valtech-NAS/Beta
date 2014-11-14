namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.Configuration;
    using Elasticsearch.Net;
    using Nest;
    using NLog;

    internal class CheckElasticsearchCluster : IMonitorTask
    {
        private const string ExpectedNodeCountSettingName = "Monitor.Elasticsearch.ExpectedNodeCount";
        private const string TimeoutSettingName = "Monitor.Elasticsearch.Timeout";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IConfigurationManager _configurationManager;

        public CheckElasticsearchCluster(IConfigurationManager configurationManager, IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _configurationManager = configurationManager;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public string TaskName
        {
            get { return "Check Elasticsearch Cluster"; }
        }

        public void Run()
        {
            var health = GetClusterHealth();

            EnsureNoTimeout(health);
            EnsureExpectedNumberOfNodes(health);
            EnsureClusterIsHealthy(health);
        }

        private IHealthResponse GetClusterHealth()
        {
            var request = new ClusterHealthRequest
            {
                Level = Level.Cluster,
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
                "Elasticsearch cluster health check timed out ({0}).", Timeout);

            throw new Exception(message);
        }

        private void EnsureExpectedNumberOfNodes(IHealthResponse response)
        {
            if (ExpectedNodeCount == response.NumberOfNodes)
            {
                return;
            }

            var message = string.Format(
                "Expected {0} Elasticsearch node(s), saw {1}.", ExpectedNodeCount, response.NumberOfNodes);

            throw new Exception(message);
        }

        private void EnsureClusterIsHealthy(IHealthResponse health)
        {
            if (health.Status == "green")
            {
                return;
            }

            var message = string.Format(
                "Cluster is unhealthy: \"{0}\".", health.Status);

            Logger.Warn(message);
        }

        private int ExpectedNodeCount
        {
            get
            {
                return int.Parse(_configurationManager.GetAppSetting(ExpectedNodeCountSettingName));
            }
        }

        private string Timeout
        {
            get
            {
                return string.Format("{0}s",
                    _configurationManager.GetAppSetting<int>(TimeoutSettingName));
            }
        }
    }
}
