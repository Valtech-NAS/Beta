namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.Entities
{
    using System;
    using Domain.Entities.Users;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoUserCredentials : UserCredentials
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}