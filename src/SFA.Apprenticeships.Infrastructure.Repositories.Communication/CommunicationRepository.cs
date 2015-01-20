namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using Domain.Entities;
    using Domain.Interfaces.Configuration;
    using Mongo.Common;
    
    public abstract class CommunicationRepository<TBaseEntity> : GenericMongoClient<TBaseEntity> where TBaseEntity : BaseEntity
    {
        protected CommunicationRepository(IConfigurationManager configurationManager, string collectionName)
            : base(configurationManager, "Communications.mongoDB", collectionName)
        {
        }
    }
}