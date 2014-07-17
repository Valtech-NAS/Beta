namespace AddressLoader.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoAddressLocation
    {
        [BsonElement("long")]
        public double Longitude { get; set; }

        [BsonElement("lat")]
        public double Latitude { get; set; }
    }
}
