namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates.Entities
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoCandidate
    {
        [BsonId]
        public int Id { get; set; }

        public string FirstName { get; set; }

        //todo: MongoCandidate props
    }
}
