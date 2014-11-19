﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.Entities
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoUserCredentials : UserCredentials
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