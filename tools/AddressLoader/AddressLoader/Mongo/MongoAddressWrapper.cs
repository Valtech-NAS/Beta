using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AddressLoader.Mongo
{
    [BsonIgnoreExtraElements]
    public class MongoAddressWrapper
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("presentation")]
        public MongoAddress Address { get; set; }

        [BsonElement("location")]
        public MongoAddressLocation Location { get; set; }
    }
}
