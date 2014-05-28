namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    using System;
    using SFA.Apprenticeships.Services.Elasticsearch.Entities.Attributes;

    public abstract class VacancyId
    {
        [ElasticsearchIdentity]
        [ElasticsearchType("long")]
        public long Id { get; set; }

        [ElasticsearchType("string")]
        public Guid UpdateReference { get; set; }
    }
}
