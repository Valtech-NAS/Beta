using System;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public abstract class VacancyId
    {
        [ElasticsearchIdentity]
        public long Id { get; set; }

        [ElasticsearchType("string")]
        public Guid UpdateReference { get; set; }
    }
}
