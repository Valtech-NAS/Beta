namespace AddressLoader.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class MongoAddress
    {
        [BsonElement("property")]
        public string AddressLine1 { get; set; }

        [BsonElement("street")]
        public string AddressLine2 { get; set; }

        [BsonElement("town")]
        public string AddressLine3 { get; set; }

        [BsonElement("area")]
        public string AddressLine4 { get; set; }

        [BsonElement("postcode")]
        public string Postcode { get; set; }
    }
}
