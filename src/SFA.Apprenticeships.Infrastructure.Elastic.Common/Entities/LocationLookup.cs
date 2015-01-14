namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "locationdatas")]
    public class LocationLookup
    {
        [ElasticProperty(Name = "name", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed)]
        public string Name { get; set; }

        [ElasticProperty(Name = "county", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed)]
        public string County { get; set; }

        [ElasticProperty(Name = "longitude", Type = FieldType.Double, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Longitude { get; set; }

        [ElasticProperty(Name = "latitude", Type = FieldType.Double, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Latitude { get; set; }

        [ElasticProperty(Name = "size", Type = FieldType.Integer, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Size { get; set; }
    }
}
