namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using System;
    using Domain.Entities;
    using Domain.Interfaces.Configuration;
    using MongoDB.Driver;

    public class GenericMongoClient<T> where T : BaseEntity
    {
        private MongoCollection<T> _collection;

        private readonly MongoDatabase _database;
        private readonly string _mongoCollectionName;

        protected MongoCollection<T> Collection
        {
            get
            {
                return _collection ?? (_collection = _database.GetCollection<T>(_mongoCollectionName));
            }
        }

        protected GenericMongoClient(IConfigurationManager configurationManager, string mongoConnectionSettingName,
            string mongoCollectionName)
        {
            _mongoCollectionName = mongoCollectionName;
            var mongoConnectionString = configurationManager.GetAppSetting(mongoConnectionSettingName);
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
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

        protected virtual void Initialise() {}
    }
}
