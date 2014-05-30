namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Service
{
    using System;
    using System.Net;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Extensions;
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Infrastructure.Common.Helpers;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Attributes;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Mapping;

    public class IndexingService<T> : IIndexingService<T>
    {
        private readonly IElasticsearchService _elasticsearchService;
        private readonly ElasticsearchMappingAttribute _mapping;

        public IndexingService(IElasticsearchService elasticsearchService)
        {
            if (elasticsearchService == null)
            {
                throw new ArgumentNullException("elasticsearchService");
            }

            _elasticsearchService = elasticsearchService;
            _mapping = GetMappingAttribute();
            Setup();
        }

        /// <summary>
        /// Checks the mappings on the es database to verify the database is setup.
        /// If not, creates the index and mappings.
        /// </summary>
        private void Setup()
        {
            var mappings = ElasticsearchMapping.Create<T>();
            var attribute = GetMappingAttribute();

            var rs = _elasticsearchService.Execute(Method.PUT, attribute.Index);
            if (rs.StatusCode != HttpStatusCode.OK)
            {
                if (!rs.Content.Contains("IndexAlreadyExistsException"))
                {
                    throw new ApplicationException(
                        string.Format("Elasticsearch service returned code '{0}' when writing index. Content: {1}", rs.StatusCode, rs.Content));
                }
            }

            rs = _elasticsearchService.Execute(attribute.Index, attribute.Document, "_mapping", mappings);
            if (rs.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(
                    string.Format("Elasticsearch service returned code '{0}' when writing mappings. Content: {1}", rs.StatusCode, rs.Content));
            }
        }

        public void Index(string id, T objectToIndex)
        {
            var json = JsonConvert.SerializeObject(objectToIndex, new EnumToStringConverter());

            var rs =
                _elasticsearchService.Execute(
                    _mapping.Index,
                    _mapping.Document,
                    id,
                    json);

            if (rs.StatusCode != HttpStatusCode.OK)
            {
                // TODO::High::Log error
            }
        }

        private static ElasticsearchMappingAttribute GetMappingAttribute()
        {
            // look for the elasticsearch mapping attribute on the class T
            // find the index and document properties to form the es command
            var mapping = typeof(T).GetAttribute<ElasticsearchMappingAttribute>();
            if (mapping == null || string.IsNullOrEmpty(mapping.Document) || string.IsNullOrEmpty(mapping.Index))
            {
                throw
                    new ArgumentException(
                        "The generic type must have the ElasticsearchMapping attribute applied with Document and Index properties.");
            }

            return mapping;
        }
    }
}
