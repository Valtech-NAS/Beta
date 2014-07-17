namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using System;
    using Domain.Interfaces.Configuration;
    using MongoDB.Driver;

    public class GenericMongoClient<T>
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
    }
}
