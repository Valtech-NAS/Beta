using System;
using Nest;

namespace AddressLoader.Process
{
    /// <summary>
    /// DTO for mapping elasticsearch index to
    /// </summary>
    [ElasticType(Name = "address")]
    public class AddressData
    {
        [ElasticProperty(Name = "addressline1", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string AddressLine1 { get; set; }

        [ElasticProperty(Name = "addressline2", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string AddressLine2 { get; set; }

        [ElasticProperty(Name = "addressline3", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string AddressLine3 { get; set; }

        [ElasticProperty(Name = "addressline4", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string AddressLine4 { get; set; }

        [ElasticProperty(Name = "longitude", Type = FieldType.double_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public double Longitude { get; set; }

        [ElasticProperty(Name = "latitude", Type = FieldType.double_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public double Latitude { get; set; }

        [ElasticProperty(Name = "uprn", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string Uprn { get; set; }

        [ElasticProperty(Name = "postcode", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.analyzed)]
        public string Postcode { get; set; }
    }
}
