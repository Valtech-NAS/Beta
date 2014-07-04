namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.Entities
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoApplicationDetail
    {
        [BsonId]
        public Guid Id { get; set; }

        //todo: MongoApplicationDetail props
    }
}
