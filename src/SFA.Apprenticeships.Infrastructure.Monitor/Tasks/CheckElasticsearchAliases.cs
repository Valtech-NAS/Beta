namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.Configuration;
    using Nest;

    internal class CheckElasticsearchAliases : IMonitorTask
    {
        private const string ExpectedAliasNamesSettingName = "Monitor.Elasticsearch.ExpectedAliasNames";

        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IConfigurationManager _configurationManager;

        public CheckElasticsearchAliases(IConfigurationManager configurationManager, IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _configurationManager = configurationManager;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public string TaskName
        {
            get { return "Check Elasticsearch Aliases"; }
        }

        public void Run()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var expectedAliasNames = GetExpectedAliasNames();

            EnsureExpectedAliasesExist(client, expectedAliasNames);
        }

        private static void EnsureExpectedAliasesExist(IElasticClient client, IEnumerable<string> expectedAliasNames)
        {
            var missingAliasNames = expectedAliasNames
                .Where(aliasName => !client.AliasExists(aliasName).Exists)
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
            return _configurationManager
                .GetAppSetting(ExpectedAliasNamesSettingName)
                .Split(new[] { ';', ',' })
                .Select(each => each.Trim().ToLower());
        }

        private static string ToCommaSeparatedString(IEnumerable<string> names)
        {
            return string.Join(", ", names);
        }
    }
}
