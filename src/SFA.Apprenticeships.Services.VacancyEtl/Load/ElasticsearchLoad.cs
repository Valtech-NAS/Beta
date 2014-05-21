using System;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Common.JsonConverters;
using SFA.Apprenticeships.Services.Elasticsearch.Mapping;
using StructureMap;

namespace SFA.Apprenticeships.Services.VacancyEtl.Load
{
    /// <summary>
    /// Monitors the queue for new data items and loads them into the es database.
    /// </summary>
    public class ElasticsearchLoad<T> where T : VacancyId
    {
        private readonly IElasticsearchService _service;

        public ElasticsearchLoad(IElasticsearchService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _service = service;
            Mapping = GetMappingAttribute();
        }

        public ElasticsearchMappingAttribute Mapping { get; private set; }

        public void Execute(VacancySummary summary)
        {
            var json = JsonConvert.SerializeObject(summary, new EnumToStringConverter());

            var rs =
                _service.Execute(
                    Mapping.Index,
                    Mapping.Document,
                    summary.Id.ToString(CultureInfo.InvariantCulture),
                    json);

            if (rs.StatusCode != HttpStatusCode.OK)
            {
                // TODO::High::Log error
            }
        }

        /// <summary>
        /// Checks the mappings on the es database to verify the database is setup.
        /// If not, creates the index and mappings.
        /// </summary>
        public static void Setup(IElasticsearchService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            var mappings = ElasticsearchMapping.Create<T>();
            var attribute = GetMappingAttribute();

            var rs = service.Execute(Method.PUT, attribute.Index);
            if (rs.StatusCode != HttpStatusCode.OK)
            {
                if (!rs.Content.Contains("IndexAlreadyExistsException"))
                {
                    throw new ApplicationException(
                        string.Format("Elasticsearch service returned code '{0}' when writing index. Content: {1}", rs.StatusCode, rs.Content));
                }
            }

            rs = service.Execute(attribute.Index, attribute.Document, "_mapping", mappings);
            if (rs.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(
                    string.Format("Elasticsearch service returned code '{0}' when writing mappings. Content: {1}", rs.StatusCode, rs.Content));
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