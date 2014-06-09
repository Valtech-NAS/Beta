namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "vacancy")]
    public class VacancySummary
    {
        public VacancySummary()
        {
            Location = new GeoPoint();
        }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public int Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public Guid UpdateReference { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string Framework { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string Title { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public VacancyType VacancyType { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public DateTime Created { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public DateTime ClosingDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string EmployerName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string ProviderName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public int NumberOfPositions { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string Description { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string AddressLine1 { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string AddressLine2 { get; set; }
        
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string AddressLine3 { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string AddressLine4 { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string AddressLine5 { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string Town { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string County { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string PostCode { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string LocalAuthority { get; set; }

        [ElasticProperty(Type = FieldType.geo_point, Index = FieldIndexOption.analyzed)]
        public GeoPoint Location { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public VacancyLocationType VacancyLocationType { get; set; }

        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public string VacancyUrl { get; set; }
    }
}