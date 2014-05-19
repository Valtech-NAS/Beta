using System;
using System.ComponentModel;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;
using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Common.Interfaces.Enums.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    [ElasticsearchMapping(Name = "vacancy", Index = "vacancies")]
    [Description("VacancySummaryData")]
    public class VacancySummary
    {
        [ElasticsearchIgnore]
        [Description("VacancyReference")]
        public ulong Id { get; set; }

        [ElasticsearchIgnore]
        public Guid UpdateReference { get; set; }

        [Description("ApprenticeshipFramework")]
        public string Framework { get; set; }

        [ElasticsearchIndex(Name = "fulltitle", Index = ElasticsearchIndexType.NotAnalyzed)]
        [Description("VacancyTitle")]
        public string Title { get; set; }

        [Description("VacancyType")]
        public VacancyType TypeOfVacancy { get; set; }

        [Description("CreatedDateTime")]
        public DateTime Created { get; set; }

        [ElasticsearchType(Name = "date", Format = "yyyy/MM/dd")]
        [Description("ClosingDate")]
        public DateTime ClosingDate { get; set; }

        [Description("EmployerName")]
        public string EmployerName { get; set; }

        [Description("LearningProviderName")]
        public string ProviderName { get; set; }

        [ElasticsearchIndex(Index = ElasticsearchIndexType.NotIndexed)]
        [Description("NumberOfPositions")]
        public int NumberOfPositions { get; set; }

        [Description("ShortDescription")]
        public string Description { get; set; }

        [Description("VacancyAddress")]
        public VacancyAddress Address { get; set; }

        [Description("VacancyLocationType")]
        public VacancyLocationType TypeOfLocation { get; set; }

        [ElasticsearchIndex(Index = ElasticsearchIndexType.NotIndexed)]
        [Description("VacancyUrl")]
        public string VacancyUrl { get; set; }
    }
}