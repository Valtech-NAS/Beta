using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AddressLoader.Mongo
{
    public class MongoAddressLocation
    {
        [BsonElement("long")]
        public double Longitude { get; set; }

        [BsonElement("lat")]
        public double Latitude { get; set; }
    }
}
