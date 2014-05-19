using System;
using System.ComponentModel;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public abstract class VacancyId
    {
        [ElasticsearchIgnore]
        [Description("VacancyReference")]
        public ulong Id { get; set; }

        [ElasticsearchIgnore]
        public Guid UpdateReference { get; set; }
    }
}
