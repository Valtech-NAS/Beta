namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using Nest;
    using Newtonsoft.Json.Converters;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration;
        private readonly ConnectionSettings _connectionSettings;

        public ElasticsearchClientFactory(ElasticsearchConfiguration elasticsearchConfiguration)
        {
            _elasticsearchConfiguration = elasticsearchConfiguration;
            _connectionSettings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _connectionSettings.AddContractJsonConverters(t => typeof(Enum).IsAssignableFrom(t) ? new StringEnumConverter() : null);
            CheckIndexes();
        }

        private void CheckIndexes()
        {
            var client = GetElasticClient();

            foreach (IElasticsearchIndexConfiguration elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                var indexExistsResponse = client.IndexExists(elasticsearchIndexConfiguration.Name);
                if (!indexExistsResponse.Exists)
                {
                    var indexSettings = new IndexSettings();
                    client.CreateIndex(elasticsearchIndexConfiguration.Name, indexSettings);
                    client.MapFromAttributes(elasticsearchIndexConfiguration.MappingType, elasticsearchIndexConfiguration.Name);
                }
            }
        }

        public ElasticClient GetElasticClient()
        {
            var client = new ElasticClient(_connectionSettings);
            return client;
        } 
    }
}
