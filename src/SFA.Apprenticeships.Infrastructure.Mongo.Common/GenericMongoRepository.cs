namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using System;
    using Infrastructure.Common.Configuration;
    using MongoDB.Driver;

    public class GenericMongoRepository<T>
    {
        protected readonly MongoCollection<T> Collection;

        protected GenericMongoRepository(IConfigurationManager configurationManager, string mongoConnectionSettingName,
            string mongoCollectionName)
        {
            var mongoConnectionString = configurationManager.GetAppSetting(mongoConnectionSettingName);
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            Collection = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName)
                .GetCollection<T>(mongoCollectionName);
        }

        //protected IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    //todo: implement predicated Get in generic mongo repo... or should move to concretes...?
        //    throw new NotImplementedException();
        //}
    }
}
