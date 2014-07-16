namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates.Entities
{
    using System;
    using Domain.Entities.Candidates;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoCandidate : Candidate
    {
        // TODO: TEMPCODE: temporarily a Guid... change to ObjectId? http://stackoverflow.com/questions/21726985/net-layered-architecture-mongodb-what-to-use-as-id
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
