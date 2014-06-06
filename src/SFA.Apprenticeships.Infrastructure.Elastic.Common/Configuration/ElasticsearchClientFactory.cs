namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nest;
    using Newtonsoft.Json.Converters;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration;
        private readonly ConnectionSettings _connectionSettings;
        private readonly Dictionary<Type, string> _typeIndexNameMap = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> _documentTypeNameMap = new Dictionary<Type, string>();

        public ElasticsearchClientFactory(ElasticsearchConfiguration elasticsearchConfiguration, bool buildIndexes = true)
        {
            _elasticsearchConfiguration = elasticsearchConfiguration;
            _elasticsearchConfiguration.Indexes.ToList().ForEach(idx => _typeIndexNameMap.Add(idx.MappingType, idx.Name));
            _elasticsearchConfiguration.Indexes.ToList()
                .ForEach(
                    idx =>
                        _documentTypeNameMap.Add(idx.MappingType,
                            ((ElasticTypeAttribute)
                                idx.MappingType.GetCustomAttributes(typeof (ElasticTypeAttribute), false)
                                    .FirstOrDefault()).Name));

            _connectionSettings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _connectionSettings.AddContractJsonConverters(t => typeof(Enum).IsAssignableFrom(t) ? new StringEnumConverter() : null);

            if (buildIndexes)
            {
                CheckIndexes();
            }
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

        public string GetIndexNameForType(Type attributeMappedType)
        {
            return _typeIndexNameMap[attributeMappedType];
        }

        public string GetDocumentNameForType(Type attributeMappedType)
        {
            return _documentTypeNameMap[attributeMappedType];
        }
    }
}
