namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates.Entities
{
    using System;
    using Domain.Entities.Candidates;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoCandidate : Candidate
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
