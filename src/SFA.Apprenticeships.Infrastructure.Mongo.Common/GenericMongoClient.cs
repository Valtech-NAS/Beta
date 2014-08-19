namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using System;
    using Domain.Entities;
    using Domain.Interfaces.Configuration;
    using MongoDB.Driver;

    public class GenericMongoClient<T> where T : BaseEntity
    {
        protected readonly MongoCollection<T> Collection;

        protected GenericMongoClient(IConfigurationManager configurationManager, string mongoConnectionSettingName,
            string mongoCollectionName)
        {
            var mongoConnectionString = configurationManager.GetAppSetting(mongoConnectionSettingName);
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            Collection = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName)
                .GetCollection<T>(mongoCollectionName);
        }

        protected void UpdateEntityTimestamps(T entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.DateCreated == DateTime.MinValue)
            {
                entity.DateCreated = DateTime.UtcNow;
                entity.DateUpdated = null;
            }
            else
            {
                entity.DateUpdated = DateTime.UtcNow;
            }
        }
    }
}
