namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "apprenticeship")]
    public class ApprenticeshipSummary : IVacancySummary
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public int Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "snowballStopwordsBase")]
        public string Title { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public DateTime StartDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public DateTime ClosingDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "keywordlowercase")]
        public string EmployerName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "snowballStopwordsExtended")]
        public string Description { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed)]
        public VacancyLocationType VacancyLocationType { get; set; }

        [ElasticProperty(Type = FieldType.GeoPoint, Index = FieldIndexOption.Analyzed)]
        public GeoPoint Location { get; set; }
        
        [ElasticProperty(Index = FieldIndexOption.Analyzed)]
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string VacancyReference { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Sector { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Framework { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string SectorCode { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string FrameworkCode { get; set; }
    }
}