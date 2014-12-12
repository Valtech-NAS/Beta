namespace SFA.Apprenticeships.Infrastructure.Repositories.Users.Entities
{
    using System;
    using Domain.Entities.Users;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoUser : User
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
