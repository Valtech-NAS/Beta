namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Elastic.Common.Configuration;
    using Nest;

    internal class CheckElasticsearchAliases : IMonitorTask
    {
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public CheckElasticsearchAliases(ElasticsearchConfiguration elasticsearchConfiguration, IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchConfiguration = elasticsearchConfiguration;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public string TaskName { get { return "Check Elasticsearch Aliases"; } }

        public void Run()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var expectedAliasNames = GetExpectedAliasNames();

            EnsureExpectedAliasesExist(client, expectedAliasNames);
        }

        private static void EnsureExpectedAliasesExist(IElasticClient client, IEnumerable<string> expectedAliasNames)
        {
            var missingAliasNames = expectedAliasNames
                .Where(aliasName => !(client.AliasExists(aliasName).Exists || client.IndexExists(aliasName).Exists))
                .ToList();

            if (missingAliasNames.Count == 0)
            {
                return;
            }

            var message = string.Format(
                "Failed to find {0} Elasticsearch alias(es): \"{1}\".",
                missingAliasNames.Count, ToCommaSeparatedString(missingAliasNames));

            throw new Exception(message);
        }

        private IEnumerable<string> GetExpectedAliasNames()
        {
            return _elasticsearchConfiguration.Indexes.Select(i => i.Name.Trim().ToLower());
        }

        private static string ToCommaSeparatedString(IEnumerable<string> names)
        {
            return string.Join(", ", names);
        }
    }
}
