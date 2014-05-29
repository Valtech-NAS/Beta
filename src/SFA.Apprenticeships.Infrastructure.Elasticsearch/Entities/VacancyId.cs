namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities
{
    using System;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Attributes;

    public abstract class VacancyId
    {
        [ElasticsearchIdentity]
        [ElasticsearchType("long")]
        public long Id { get; set; }

        [ElasticsearchType("string")]
        public Guid UpdateReference { get; set; }
    }
}
