namespace SFA.Apprenticeships.Infrastructure.Repositories.Users.Entities
{
    using System;
    using Domain.Entities.Users;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoUser : User
    {
        //todo: temporarily a Guid... change to ObjectId? http://stackoverflow.com/questions/21726985/net-layered-architecture-mongodb-what-to-use-as-id
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
