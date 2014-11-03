namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    /// <summary>
    /// DTO for mapping elasticsearch index to
    /// </summary>
    [ElasticType(Name = "address")]
    public class Address
    {
        [ElasticProperty(Name = "addressline1", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string AddressLine1 { get; set; }

        [ElasticProperty(Name = "addressline2", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string AddressLine2 { get; set; }

        [ElasticProperty(Name = "addressline3", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string AddressLine3 { get; set; }

        [ElasticProperty(Name = "addressline4", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string AddressLine4 { get; set; }

        [ElasticProperty(Name = "longitude", Type = FieldType.Double, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Longitude { get; set; }

        [ElasticProperty(Name = "latitude", Type = FieldType.Double, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Latitude { get; set; }

        [ElasticProperty(Name = "uprn", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string Uprn { get; set; }

        [ElasticProperty(Name = "postcode", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed)]
        public string Postcode { get; set; }

        [ElasticProperty(Name = "postcodesearch", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed)]
        public string PostcodeSearch { get; set; }
    }
}
