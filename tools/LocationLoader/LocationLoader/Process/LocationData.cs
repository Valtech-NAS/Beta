namespace LocationLoader.Process
{
    using Nest;

    /// <summary>
    /// DTO for mapping elasticsearch index to
    /// </summary>
    [ElasticType(Name = "locationdatas")]
    internal class LocationData
    {
        [ElasticProperty(Name = "name", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed, Analyzer = "keywordlowercase")]
        public string Name { get; set; }

        [ElasticProperty(Name = "county", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed)]
        public string County { get; set; }

        [ElasticProperty(Name = "country", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string Country { get; set; }

        [ElasticProperty(Name = "longitude", Type = FieldType.Double, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Longitude { get; set; }

        [ElasticProperty(Name = "latitude", Type = FieldType.Double, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public double Latitude { get; set; }

        [ElasticProperty(Name = "postcode", Type = FieldType.String, Store = true, Index = FieldIndexOption.Analyzed)]
        public string Postcode { get; set; }

        [ElasticProperty(Name = "type", Type = FieldType.String, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public string Type { get; set; }

        [ElasticProperty(Name = "size", Type = FieldType.Integer, Store = true, Index = FieldIndexOption.NotAnalyzed)]
        public int Size
        {
            get
            {
                switch (Type)
                {
                    case "City":
                        return 5;
                    case "Town":
                        return 3;
                    case "Other":
                        return 1;
                    default:
                        return 1;
                }
            }
        }
    }
}
