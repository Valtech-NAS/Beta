namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication.Entities
{
    using System;
    using Domain.Entities.Communication;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoApprenticeshipApplicationExpiringDraft : ExpiringApprenticeshipApplicationDraft
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
