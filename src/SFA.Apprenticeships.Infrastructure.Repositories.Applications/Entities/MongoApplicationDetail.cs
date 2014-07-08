namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.Entities
{
    using System;
    using Domain.Entities.Applications;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoApplicationDetail : ApplicationDetail
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
