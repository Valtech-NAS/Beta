using System;
using Nest;

namespace LocationLoader.Process
{
    /// <summary>
    /// DTO for mapping elasticsearch index to
    /// </summary>
    internal class LocationData
    {
        private string _name;

        [ElasticProperty(Name = "name", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.analyzed, Analyzer = "keyword")]
        public string Name
        {
            get { return _name.ToLower(); }
            set { _name = value; }
        }

        [ElasticProperty(Name = "county", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.analyzed)]
        public string County { get; set; }

        [ElasticProperty(Name = "country", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string Country { get; set; }

        [ElasticProperty(Name = "longitude", Type = FieldType.double_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public double Longitude { get; set; }

        [ElasticProperty(Name = "latitude", Type = FieldType.double_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public double Latitude { get; set; }

        [ElasticProperty(Name = "postcode", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.analyzed)]
        public string Postcode { get; set; }

        [ElasticProperty(Name = "type", Type = FieldType.string_type, Store = true, Index = FieldIndexOption.not_analyzed)]
        public string Type { get; set; }

        [ElasticProperty(Name = "size", Type = FieldType.integer_type, Store = true, Index = FieldIndexOption.not_analyzed)]
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
