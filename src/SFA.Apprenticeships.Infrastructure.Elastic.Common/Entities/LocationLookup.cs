namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "locationdatas")]
    public class LocationLookup
    {
        [ElasticProperty(Name = "name", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.analyzed)]
        public string Name { get; set; }

        [ElasticProperty(Name = "county", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.analyzed)]
        public string County { get; set; }

        [ElasticProperty(Name = "longitude", Type = FieldType.double_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public double Longitude { get; set; }

        [ElasticProperty(Name = "latitude", Type = FieldType.double_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public double Latitude { get; set; }
    }
}
