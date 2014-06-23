namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "vacancy")]
    public class VacancySummary
    {
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public int Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string Title { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public DateTime ClosingDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string EmployerName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string Description { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public VacancyLocationType VacancyLocationType { get; set; }

        [ElasticProperty(Type = FieldType.geo_point, Index = FieldIndexOption.analyzed)]
        public GeoPoint Location { get; set; }
    }
}