namespace AddressLoader.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// Wrapper DTO to ease deserialisation of address data from the source database
    /// </summary>
    [BsonIgnoreExtraElements]
    public class MongoAddressWrapper
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("uprn")]
        public string Uprn { get; set; }

        [BsonElement("presentation")]
        public MongoAddress Address { get; set; }

        [BsonElement("location")]
        public MongoAddressLocation Location { get; set; }
    }
}
