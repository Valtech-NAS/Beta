namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.Entities
{
    using System;
    using Domain.Entities.Applications;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoTraineeshipApplicationDetail : TraineeshipApplicationDetail
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}

