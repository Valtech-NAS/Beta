using System;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public abstract class VacancyId
    {
        [ElasticsearchIdentity]
        [ElasticsearchType("long")]
        public ulong Id { get; set; }

        [ElasticsearchType("string")]
        public Guid UpdateReference { get; set; }
    }
}
